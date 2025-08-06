using MedicoAPI.Models.DTO.Patient;

namespace MedicoAPI.Models.DTO.PatientAssessment
{
    public class PatientAsessmentChartDTO
    {
        public string AssessmentDescription { get; set; }
        public string MedicalHistoryDescription { get; set; }
        
        public PatientAsessmentProfileDTO Patient { get; set; }
    }
}