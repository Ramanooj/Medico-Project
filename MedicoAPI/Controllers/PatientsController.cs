using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using MedicoAPI.Data;
using MedicoAPI.Models;
using MedicoAPI.Models.DTO.Patient;
using MedicoAPI.Models.DTO.Prescription;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace MedicoAPI.Controllers
{
    [Authorize(Roles = "patient")]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly MedicoAPIContext _context;
        private readonly IAmazonCognitoIdentityProvider _cognitoClient;
        public PatientsController(MedicoAPIContext context, IAmazonCognitoIdentityProvider cognitoClient)
        {
            _context = context;
            _cognitoClient = cognitoClient;
        }

        [HttpPost("Sign-up")]
        public async Task<ActionResult> PostPatient([FromBody] PatientCreationDTO newPatient, [FromHeader(Name = "X-ID-Token")] string idToken)
        {
            if (string.IsNullOrEmpty(getPatientId()))
            {
                ModelState.AddModelError("Error", "Patient ID not found in the access token");
                return BadRequest(ModelState);
            }

            var existingPatient = PatientExists(getPatientId());
            if (existingPatient)
            {
                ModelState.AddModelError("Error", "User already exists");
                return BadRequest(ModelState);
            }

            JwtSecurityToken jwtToken;
            try
            {
                jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(idToken);
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

            if (ModelState.IsValid)
            {
                var completeFirstName = !string.IsNullOrEmpty(middleName) && middleName.ToLower() != "n/a" ? $"{firstName} {middleName}" : firstName;

                var patient = new Patient
                {
                    PatientId = getPatientId(),
                    Email = email,
                    FirstName = completeFirstName,
                    LastName = lastName,
                    PhoneNumber = phoneNumber,
                    DateOfBirth = newPatient.DateOfBirth,
                    HealthCardNumber = newPatient.HealthCardNumber,
                    PatientAddress = newPatient.ptFullAddress,
                    Gender = newPatient.gender,
                    Allergies = newPatient.Allergies.Select(a => new Allergy
                    {
                        PatientId = getPatientId(),
                        AllergyName = a.AllergyName
                    }).ToList(),
                    Medications = newPatient.Medications.Select(m => new Medication
                    {
                        PatientId = getPatientId(),
                        MedicationDescription = m.MedicationDescription
                    }).ToList()
                };

                _context.Patient.Add(patient);
                await _context.SaveChangesAsync();

                return Ok();
            }

            ModelState.AddModelError("Error", "Invalid model state");
            return BadRequest(ModelState);
        }

        [HttpPost("Allergy")]
        public async Task<ActionResult> PostAllergy(AllergyDTO newAllergy)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Error", "Invalid model state");
                return BadRequest(ModelState);
            }
            var existingAllergies = await _context.Allergies.AsNoTracking().Where(allrgy => allrgy.PatientId == getPatientId()).ToListAsync();

            if (existingAllergies.Any(allrgy => allrgy.AllergyName == newAllergy.AllergyName))
            {
                ModelState.AddModelError("Error", "Type of Allergy Already Exists!");
                return BadRequest(ModelState);
            }

            _context.Allergies.Add(new Allergy
            {
                AllergyName = newAllergy.AllergyName,
                PatientId = getPatientId(),
            });
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("Medication")]
        public async Task<ActionResult> PostMedication(MedicationDTO newMedication)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Error", "Invalid model state");
                return BadRequest(ModelState);
            }
            var existingMedication = await _context.Medications.AsNoTracking().Where(med => med.PatientId == getPatientId()).ToListAsync();

            if (existingMedication.Any(med => med.MedicationDescription == newMedication.MedicationDescription))
            {
                ModelState.AddModelError("Error", "Type of Medication Already Exists!");
                return BadRequest(ModelState);
            }

            _context.Medications.Add(new Medication
            {
                MedicationDescription = newMedication.MedicationDescription,
                PatientId = getPatientId(),
            });
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("Dashboard")]
        public async Task<ActionResult<PatientDashboardDTO>> GetPatientDashboardData()
        {
            var patientId = User.FindFirst("username")?.Value;

            if (PatientExists(patientId))
            {
                PatientDashboardDTO patientDashBoard = new();

                var appointments = await _context.Appointment
                                  .AsNoTracking()
                                  .Where(app => app.PatientId == patientId)
                                  .Include(app => app.Doctor)
                                  .ToListAsync();

                patientDashBoard.Appointments = appointments
                                                  .Where(app => app.AppmntDate.ToDateTime(app.AppmntTime) > DateTime.Now)
                                                  .Select(app => new PatientDashBoardAppointmentDTO
                                                  {
                                                      AppointmentId = app.AppointmentId,
                                                      Reason = app.Reason,
                                                      DoctorFullName = $"Dr. {app.Doctor.FirstName} {app.Doctor.LastName}",
                                                      appointmentSchedule = app.AppmntDate.ToDateTime(app.AppmntTime)
                                                  })
                                                  .ToList();

                patientDashBoard.Prescriptions = await _context.Prescription
                                                .AsNoTracking()
                                                .Where(pres => pres.PatientId == patientId)
                                                .Include(pres => pres.Doctor)
                                                .Select(pres =>
                                                     new PatientDashboardPrescription
                                                     {
                                                         PrescriptionId = pres.PrescriptionId,
                                                         PrescriptionContent = pres.PrescriptionContent,
                                                         IssueDate = pres.IssueDate,
                                                         DoctorName = $"Dr. {pres.Doctor.FirstName} {pres.Doctor.LastName}"
                                                     }
                                                ).ToListAsync();

                return Ok(patientDashBoard);
            }
            ModelState.AddModelError("Error", "Patient Does not Exists");
            return BadRequest(ModelState);
        }

        [HttpGet("isPatientRegistered")]
        public async Task<ActionResult> isPatientRegistered()
        {
            return Ok(new { isRegistered = PatientExists(getPatientId()) });
        }

        [HttpGet("Profile")]
        public async Task<ActionResult<PatientProfileDTO>> GetPatient()
        {
            var patient = await _context.Patient.AsNoTracking()
                .Include(p => p.Allergies)
                .Include(p => p.Medications)
                .FirstOrDefaultAsync(p => p.PatientId == getPatientId());

            if (patient == null)
            {
                return NotFound();
            }

            var patientDTO = new PatientProfileDTO
            {
                PatientId = patient.PatientId,
                Email = patient.Email,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                PhoneNumber = patient.PhoneNumber,
                DateOfBirth = patient.DateOfBirth,
                HealthCardNumber = patient.HealthCardNumber,
                PatientAddress = patient.PatientAddress,
                gender = patient.Gender,

                Allergies = patient.Allergies.Select(a => new PatientProfileAllergyDTO
                {
                    AllergyId = a.AllergyId,
                    AllergyName = a.AllergyName
                }).ToList(),

                Medications = patient.Medications.Select(m => new PatientProfileMedsDTO
                {
                    MedicationId = m.MedicationId,
                    MedicationDescription = m.MedicationDescription
                }).ToList()
            };

            return Ok(patientDTO);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> PutPatient(PatientUpdateDTO patient)
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
                var pt = await _context.Patient.FindAsync(getPatientId());
                if (pt != null)
                {
                    // Synchronize Email
                    if (pt.Email != patient.Email)
                    {
                        try
                        {
                            var updateRequest = new UpdateUserAttributesRequest
                            {
                                AccessToken = accessToken,
                                UserAttributes = new List<AttributeType>
                                {
                                    new AttributeType { Name = "email", Value = patient.Email }
                                }
                            };
                            await _cognitoClient.UpdateUserAttributesAsync(updateRequest);
                        }

                        catch (Exception ex)
                        {
                            ModelState.AddModelError("Error", "An error occurred while updating Cognito: " + ex.Message);
                            return BadRequest(ModelState);
                        }
                    }

                    // Update the local database
                    pt.Email = patient.Email;
                    pt.PatientAddress = patient.PatientAddress;

                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!PatientExists(getPatientId()))
                        {
                            ModelState.AddModelError("Error", "Patient may not exist");
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



        // DELETE A USER AND WILL ONLY PERFORM WHEN PASSED IT A CONFIRMATION STRING 
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeletePatient([FromQuery] string confirmation)
        {

            if (confirmation != "CONFIRMED")
            {
                ModelState.AddModelError("Error", "Deletion not confirmed");
                return BadRequest(ModelState);
            }

            var patient = await _context.Patient.FindAsync(getPatientId());
            if (patient == null)
            {
                return NotFound("Patient does not exist!");
            }
            _context.Patient.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();

        }

        [HttpDelete("Allergy/Delete")]
        public async Task<IActionResult> DeleteAllergy([FromQuery] string allergyId)
        {
            var allergy = await _context.Allergies.FindAsync(allergyId);
            if (allergy == null)
            {
                ModelState.AddModelError("Error", "Allergy id does not exists");
                return BadRequest(ModelState);
            }

            _context.Allergies.Remove(allergy);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("Medication/Delete")]
        public async Task<IActionResult> DeleteMedication([FromQuery] string medicationId)
        {
            var med = await _context.Medications.FindAsync(medicationId);
            if (med == null)
            {
                ModelState.AddModelError("Error", "Allergy id does not exists");
                return BadRequest(ModelState);
            }

            _context.Medications.Remove(med);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool PatientExists(string patientID)
        {
            return _context.Patient.AsNoTracking().Any(e => e.PatientId == patientID);
        }


        private string getPatientId()
        {
            return User.FindFirst("username").Value;
        }
    }
}