using MedicoAPI.Data;
using MedicoAPI.Models.DTO.PatientAssessment;
using MedicoAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicoAPI.Models.DTO.MedicalHistory;

namespace MedicoAPI.Controllers
{
    [Authorize(Roles = "doctor")]
    [Route("api/[controller]")]
    [ApiController]
    public class AssessmentsController : ControllerBase
    {
        private readonly MedicoAPIContext _context;
        public AssessmentsController(MedicoAPIContext context)
        {
            _context = context;
        }

        [HttpPost("Assessment")]
        public async Task<ActionResult> PostPatientAssessment(PatientAssessmentCreationDTO newAssessement)
        {
            if (!DoctorExists() || !isDoctorVerified())
            {
                ModelState.AddModelError("Error", "Doctor does not or unverfied");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Error", "Invalid parameters");
                return BadRequest(ModelState);
            }
            if (!PatientExists(newAssessement.PatientId))
            {
                ModelState.AddModelError("Error", "Patient Does not Exists");
                return BadRequest(ModelState);
            }

            var ptAssessment = new PatientAssessment
            {
                PatientId = newAssessement.PatientId,
                AssessmentDescription = newAssessement.AssessmentDescription,
                DoctorId = getDoctorId(),
            };

            try
            {
                _context.Assessments.Add(ptAssessment);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost("MedicalHistory")]
        public async Task<ActionResult> PostMedicalHistory(MedicalHistoryCreationDTO newMedicalEntree)
        {
            if (!DoctorExists() || !isDoctorVerified())
            {
                ModelState.AddModelError("Error", "Doctor does not or unverfied");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Error", "Invalid parameters");
                return BadRequest(ModelState);
            }
            if (!PatientExists(newMedicalEntree.PatientId))
            {
                ModelState.AddModelError("Error", "Patient Does not Exists");
                return BadRequest(ModelState);
            }

            var newEntree = new MedicalHistory
            {
                PatientId = newMedicalEntree.PatientId,
                Description = newMedicalEntree.AssessmentDescription,
                DoctorId = getDoctorId()
            };

            try
            {
                _context.MedicalHistories.Add(newEntree);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpGet("MedicalHistory")]
        public async Task<ActionResult<MedicalHistoryDTO>> GetPatientMedicalHistories([FromQuery] string patientId)
        {
            if (!DoctorExists() || !isDoctorVerified())
            {
                ModelState.AddModelError("Error", "Doctor does not or unverfied");
                return BadRequest(ModelState);
            }

            if (!PatientExists(patientId))
            {
                ModelState.AddModelError("Error", "Patient Does not Exists");
                return BadRequest(ModelState);
            }

            var ptUpdatedMedicalHistory = new MedicalHistoryDTO();

            var ptMedicalHistories = await _context.MedicalHistories
            .AsNoTracking()
            .Include(mh => mh.Patient)
            .Where(mh => mh.PatientId == patientId && mh.DoctorId == getDoctorId())
            .OrderByDescending(mh => mh.LastUpdated)
            .ToListAsync();

            if (ptMedicalHistories != null && ptMedicalHistories.Any())
            {
                var latestMedicalHistory = ptMedicalHistories[0];

                ptUpdatedMedicalHistory.PatientId = latestMedicalHistory.PatientId;
                ptUpdatedMedicalHistory.PatientName = $"{latestMedicalHistory.Patient.FirstName} {latestMedicalHistory.Patient.LastName}";
                ptUpdatedMedicalHistory.MedicalHistoryDescription = latestMedicalHistory.Description;

            }
            else
            {
                var patient = await _context.Patient.AsNoTracking().Where(p => p.PatientId == patientId).FirstAsync();
                ptUpdatedMedicalHistory.PatientName = $"{patient.FirstName} {patient.LastName}";
                ptUpdatedMedicalHistory.MedicalHistoryDescription = "You're the first one!";
                ptUpdatedMedicalHistory.PatientId = patientId;
            }
            return Ok(ptUpdatedMedicalHistory);

        }

        [HttpGet("Patients")]
        public async Task<ActionResult<List<AssociatedPatientsDTO>>> GetPatients()
        {

            if (!DoctorExists() || !isDoctorVerified())
            {
                ModelState.AddModelError("Error", "Doctor does not or unverfied");
                return BadRequest(ModelState);
            }

            var associatedPatients = await _context.Appointment
                .AsNoTracking()
                .Include(app => app.Patient)
                .Where(app => app.DoctorId == getDoctorId())
                .Select(app => new AssociatedPatientsDTO
                {
                    PatientId = app.PatientId,
                    PatientFullName = $"{app.Patient.FirstName} {app.Patient.LastName}"
                })
                .ToListAsync();

            return Ok(associatedPatients);
        }

        [HttpGet("PatientAssessments")]
        public async Task<ActionResult<List<PatientAssessmentDTO>>> GetAllPatientAssessments([FromQuery] string patientId)
        {
            if (!DoctorExists() || !isDoctorVerified())
            {
                ModelState.AddModelError("Error", "Doctor does not or unverfied");
                return BadRequest(ModelState);
            }

            if (!PatientExists(patientId))
            {
                ModelState.AddModelError("Error", "Patient Does not exists");
                return BadRequest(ModelState);
            }

            var assessments = await _context.Assessments
                .AsNoTracking()
                .Include(ass => ass.Patient)
                .Where(ass => ass.PatientId == patientId && ass.DoctorId == getDoctorId())
                .Distinct()
                .ToListAsync();

            var ptAssessments = assessments.Select(ass =>
            new PatientAssessmentDTO
            {
                AssessmentId = ass.AssessmentId,
                PatientId = ass.PatientId,
                PatientName = $"{ass.Patient.FirstName} {ass.Patient.LastName}",
                AssessmentDescription = ass.AssessmentDescription

            }).ToList();

            return Ok(ptAssessments);
        }

        [HttpGet("Chart")]
        public async Task<ActionResult<PatientAsessmentChartDTO>> GetPatientChart([FromQuery] string patientId)
        {
            if (!DoctorExists() || !isDoctorVerified())
            {
                ModelState.AddModelError("Error", "Doctor does not or unverfied");
                return BadRequest(ModelState);
            }

            if (!PatientExists(patientId))
            {
                ModelState.AddModelError("Error", "Invalid Patient id");
                return BadRequest(ModelState);
            }


            var patientChart = new PatientAsessmentChartDTO();

            var ptMedicalHistories = await _context.MedicalHistories
                    .AsNoTracking()
                    .Where(mh => mh.PatientId == patientId && mh.DoctorId == getDoctorId())
                    .OrderByDescending(mh => mh.LastUpdated)
                    .ToListAsync();

            if (ptMedicalHistories != null && ptMedicalHistories.Any())
            {
                var latestMedicalHistory = ptMedicalHistories[0];
                patientChart.MedicalHistoryDescription = latestMedicalHistory.Description;

            }
            else { patientChart.MedicalHistoryDescription = "No record yet!"; }


            var patientAssessments = await _context.Assessments
                    .AsNoTracking()
                    .Include(ass => ass.Patient)
                    .ThenInclude(patient => patient.Allergies)
                    .Include(ass => ass.Patient)
                    .ThenInclude(patient => patient.Medications)
                    .Include(ass => ass.Patient.Medications)
                    .Where(ass => ass.PatientId == patientId && ass.DoctorId == getDoctorId())
                    .OrderByDescending(ass => ass.CreatedDate)
                    .ToListAsync();

            if (patientAssessments != null && patientAssessments.Any())
            {
                var latestAssessment = patientAssessments[0];

                patientChart.AssessmentDescription = latestAssessment.AssessmentDescription;

                patientChart.Patient = new PatientAsessmentProfileDTO
                {
                    FirstName = latestAssessment.Patient.FirstName,
                    LastName = latestAssessment.Patient.LastName,
                    Email = latestAssessment.Patient.Email,
                    PatientAddress = latestAssessment.Patient.PatientAddress,
                    PhoneNumber = latestAssessment.Patient.PhoneNumber,
                    DateOfBirth = latestAssessment.Patient.DateOfBirth,
                    Gender = latestAssessment.Patient.Gender,
                    HealthCardNumber = latestAssessment.Patient.HealthCardNumber,

                    Allergies = latestAssessment.Patient.Allergies
                        .Select(pt => new PatientAssessmentProfileAllergyDTO { AllergyName = pt.AllergyName })
                        .ToList(),
                    Medications = latestAssessment.Patient.Medications
                        .Select(pt => new PatientAssessmentProfileMedsDTO { MedicationDescription = pt.MedicationDescription })
                        .ToList()
                };

            }

            return Ok(patientChart);
        }

        private string getDoctorId()
        {
            return User.FindFirst("username").Value;
        }

        private bool isDoctorVerified()
        {
            return _context.Doctor.Find(getDoctorId())?.IsVerified ?? false;
        }

        private bool DoctorExists()
        {
            return _context.Doctor.Any(e => e.DoctorId == getDoctorId());
        }

        private bool PatientExists(string id)
        {
            return _context.Patient.Any(e => e.PatientId == id);
        }


    }
}
