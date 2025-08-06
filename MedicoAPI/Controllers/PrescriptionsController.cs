using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicoAPI.Data;
using MedicoAPI.Models;
using MedicoAPI.Models.DTO.Prescription;
using Microsoft.AspNetCore.Authorization;
using MedicoAPI.Models.DTO.PatientAssessment;

namespace MedicoAPI.Controllers
{
    // GET A LIST OF PRESCRIPTION FOR A PATIENT
    [Authorize(Roles = "doctor")]
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        private readonly MedicoAPIContext _context;

        public PrescriptionsController(MedicoAPIContext context)
        {
            _context = context;
        }

        [HttpPost("Prescribe")]
        public async Task<ActionResult> PostPrescription(NewPrescription nPrescription)
        {
            if (!DoctorExists() || !isDoctorVerified())
            {
                ModelState.AddModelError("Error", "Doctor is unverfied");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Error", "Invalid Parameters");
                return BadRequest(ModelState);
            }

            try
            {
                var prescription = new Prescription
                {
                    DoctorId = getDoctorId(),
                    PatientId = nPrescription.PatientId,
                    PrescriptionContent = nPrescription.PrescriptionContent,
                    RepeatNum = nPrescription.RepeatNum,
                    DaysApart = nPrescription.DaysApart
                };
                _context.Prescription.Add(prescription);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpGet("Patients")]
        public async Task<ActionResult<List<AssociatedPatientsDTO>>> GetPatients()
        {
            if (!DoctorExists() || !isDoctorVerified())
            {
                ModelState.AddModelError("Error", "Doctor is unverfied");
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
                }).Distinct()
                .ToListAsync();

            return Ok(associatedPatients);
        }

        [HttpGet("Prescriptions")]
        public async Task<ActionResult<List<FullDetailPrescription>>> GetAllPrescription([FromQuery] string patientId)
        {
            if (!DoctorExists() || !isDoctorVerified())
            {
                ModelState.AddModelError("Error", "Doctor is unverfied");
                return BadRequest(ModelState);
            }

            if (!PatientExists(patientId))
            {
                ModelState.AddModelError("Error", "Patient does not exsits");
                return BadRequest(ModelState);
            }

            var prescriptionList = await _context.Prescription
                             .AsNoTracking()
                             .Where(pres => pres.PatientId == patientId && pres.DoctorId == getDoctorId())
                             .Include(app => app.Patient)
                             .Include(app => app.Doctor)
                             .Select(pres => new FullDetailPrescription
                             {
                                 PrescriptionId = pres.PrescriptionId,
                                 PatientName = $"{pres.Patient.FirstName} {pres.Patient.LastName}",
                                 DoctorName = $"{pres.Doctor.FirstName} {pres.Doctor.LastName}",
                                 DoctorCPSONum = pres.Doctor.DoctorCPSONum,
                                 DoctorPhone = pres.Doctor.DoctorPhone,
                                 PrescriptionContent = pres.PrescriptionContent,
                                 RepeatNum = pres.RepeatNum,
                                 DaysApart = pres.DaysApart,
                                 IssueDate = pres.IssueDate
                             }).ToListAsync();

            return Ok(prescriptionList);
        }


        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAPrescription([FromQuery] string prescriptionId)
        {
            if (!DoctorExists() || !isDoctorVerified())
            {
                ModelState.AddModelError("Error", "Doctor is unverfied");
                return BadRequest(ModelState);
            }

            var prescription = await _context.Prescription.FindAsync(prescriptionId);
            if (prescription == null)
            {
                return NotFound();
            }

            _context.Prescription.Remove(prescription);
            await _context.SaveChangesAsync();

            return Ok();
        }


        private async Task<FullDetailPrescription> GetPrescriptionDetails(string prescriptionID)
        {
            var selectedPrescription = new FullDetailPrescription();

            var prescription = await _context.Prescription
                .AsNoTracking()
                .Include(pres => pres.Doctor)
                .Include(pres => pres.Patient)
                .FirstOrDefaultAsync(pres => pres.PrescriptionId == prescriptionID);

            if (prescription != null)
            {
                selectedPrescription.DoctorName = $"Dr. {prescription.Doctor.FirstName} {prescription.Doctor.LastName}";
                selectedPrescription.DoctorCPSONum = prescription.Doctor.DoctorCPSONum;
                selectedPrescription.DoctorPhone = prescription.Doctor.DoctorPhone;
                selectedPrescription.PrescriptionContent = prescription.PrescriptionContent;
                selectedPrescription.PatientName = $"{prescription.Patient.FirstName} {prescription.Patient.LastName}";
                selectedPrescription.IssueDate = prescription.IssueDate;
                selectedPrescription.RepeatNum = prescription.RepeatNum;
                selectedPrescription.DaysApart = prescription.DaysApart;
                selectedPrescription.PrescriptionId = prescription.PrescriptionId;
            };

            return selectedPrescription;
        }

        private bool PrescriptionExists(string prescriptionID)
        {
            return _context.Prescription.Any(e => e.PrescriptionId == prescriptionID);
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

        private bool PatientExists(string patientID)
        {
            return _context.Patient.AsNoTracking().Any(e => e.PatientId == patientID);
        }

    }
}

