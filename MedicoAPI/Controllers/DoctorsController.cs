using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicoAPI.Data;
using MedicoAPI.Models;
using MedicoAPI.Models.DTO.Doctor;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.CognitoIdentityProvider;

namespace MedicoAPI.Controllers
{
    [Authorize(Roles = "doctor")]
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly MedicoAPIContext _context;
        private readonly IAmazonCognitoIdentityProvider _cognitoClient;

        public DoctorsController(MedicoAPIContext context, IAmazonCognitoIdentityProvider cognitoClient)
        {
            _context = context;
            _cognitoClient = cognitoClient;
        }


        [HttpPost("Sign-up")]
        public async Task<ActionResult> PostDoctor([FromBody] DoctorCreationDTO newDoctor, [FromHeader(Name = "X-ID-Token")] string idToken)
        {
            string doctorId = getDoctorId();
            if (string.IsNullOrEmpty(doctorId))
            {
                ModelState.AddModelError("Error", "Doctor ID not found in the access token");
                return BadRequest(ModelState);
            }

            var existingDoctor = await _context.Doctor.FirstOrDefaultAsync(d => d.DoctorId == doctorId);
            if (existingDoctor != null)
            {
                ModelState.AddModelError("Error", "User already exists");
                return BadRequest(ModelState);
            }

            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken;
            try
            {
                jwtToken = handler.ReadJwtToken(idToken);
            }
            catch (Exception)
            {
                ModelState.AddModelError("Error", "Invalid ID token");
                return BadRequest(ModelState);
            }

            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            var firstName = jwtToken.Claims.FirstOrDefault(c => c.Type == "given_name")?.Value;
            var middleName = jwtToken.Claims.FirstOrDefault(c => c.Type == "middle_name")?.Value;
            var lastName = jwtToken.Claims.FirstOrDefault(c => c.Type == "family_name")?.Value;
            var phoneNumber = jwtToken.Claims.FirstOrDefault(c => c.Type == "phone_number")?.Value;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(phoneNumber))
            {
                ModelState.AddModelError("Error", "Missing required information in the ID token");
                return BadRequest(ModelState);
            }

            if (ModelState.IsValid)
            {
                var completeFirstName = !string.IsNullOrEmpty(middleName) && middleName.ToLower() != "n/a" ? $"{firstName} ${middleName}" : firstName;
                var doctor = new Doctor
                {
                    DoctorId = doctorId,
                    Email = email,
                    FirstName = completeFirstName,
                    LastName = lastName,
                    DoctorPhone = phoneNumber,
                    Specialty = newDoctor.Specialty,
                    ClinicAddress = newDoctor.ClinicAddress,
                    DoctorCPSONum = newDoctor.DoctorCPSONum,
                    IsVerified = false
                };

                _context.Doctor.Add(doctor);
                await _context.SaveChangesAsync();

                return Ok();
            }

            ModelState.AddModelError("Error", "Invalid model state");
            return BadRequest(ModelState);
        }

        [HttpGet("isDoctorRegistered")]
        public async Task<ActionResult> isDoctorRegistered()
        {
            return Ok(new { isRegistered = DoctorExists(getDoctorId()) });
        }

        [HttpGet("Dashboard")]
        public async Task<ActionResult<List<AppointmentDoctorDashboardDTO>>> DoctorDashboard()
        {
            var doctorID = getDoctorId();

            if (DoctorExists(doctorID))
            {
                var docAppointments = await _context.Appointment
                                                .AsNoTracking()
                                                .Where(app => app.DoctorId == doctorID)
                                                .Include(app => app.Patient)
                                                .Select(app =>
                                                    new AppointmentDoctorDashboardDTO
                                                    {
                                                        AppointmentId = app.AppointmentId,
                                                        Reason = app.Reason,
                                                        PatientFullName = $"{app.Patient.FirstName} {app.Patient.LastName}",
                                                        AppmntDate = app.AppmntDate,
                                                        AppmntTime = app.AppmntTime
                                                    }
                                                )
                                                .OrderByDescending(app => app.AppmntDate)
                                                .ThenByDescending(app => app.AppmntTime)
                                                .ToListAsync();
                return Ok(docAppointments);
            }
            ModelState.AddModelError("Error", "Doctor does not exists");
            return BadRequest(ModelState);
        }

        [HttpGet("Profile")]
        public async Task<ActionResult<DoctorProfileDTO>> GetDoctor()
        {
            var doctor = await _context.Doctor.FindAsync(getDoctorId());

            if (doctor == null)
            {
                return NotFound();
            }

            var doctorDTO = new DoctorProfileDTO
            {

                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Email = doctor.Email,
                DoctorPhone = doctor.DoctorPhone,
                DoctorCPSONum = doctor.DoctorCPSONum,
                Specialty = doctor.Specialty,
                ClinicAddress = doctor.ClinicAddress
            };

            return Ok(doctorDTO);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> PutDoctor(DoctorUpdateDTO doctor)
        {
            // Retrieve the access token from the request's Authorization header
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                ModelState.AddModelError("Error", "Access token is missing");
                return BadRequest(ModelState);
            }
            var accessToken = authorizationHeader.Substring("Bearer ".Length).Trim();

            if (ModelState.IsValid)
            {
                var doc = await _context.Doctor.FindAsync(getDoctorId());

                if (doc != null)
                {
                    try
                    {
                        var updateRequest = new UpdateUserAttributesRequest
                        {
                            AccessToken = accessToken,
                            UserAttributes = new List<AttributeType>
                                {
                                    new AttributeType { Name = "email", Value = doctor.Email }
                                }
                        };
                        await _cognitoClient.UpdateUserAttributesAsync(updateRequest);
                    }

                    catch (Exception ex)
                    {
                        ModelState.AddModelError("Error", "An error occurred while updating Cognito: " + ex.Message);
                        return BadRequest(ModelState);
                    }

                    doc.Email = doctor.Email;
                    doc.ClinicAddress = doctor.ClinicAddress;
                    doc.Specialty = doctor.Specialty;

                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!DoctorExists(getDoctorId()))
                        {
                            ModelState.AddModelError("Error", "Doctor may not exist");
                            return BadRequest(ModelState);
                        }
                        else
                        {
                            throw;
                        }
                    }

                    return Ok();
                }

            }
            ModelState.AddModelError("Error", "Invalid model state");
            return BadRequest(ModelState);
        }

        private string getDoctorId()
        {
            return User.FindFirst("username").Value;
        }

        private bool DoctorExists(string id)
        {
            return _context.Doctor.Any(e => e.DoctorId == id);
        }

    }
}