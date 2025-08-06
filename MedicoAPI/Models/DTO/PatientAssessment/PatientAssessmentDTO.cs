namespace MedicoAPI.Models.DTO.PatientAssessment
{
    public class PatientAssessmentDTO
    {
        public string AssessmentId { get; set; }
        public DateTime AssessmentCreationDate { get; set; }
        public string PatientName { get; set; }
        public string PatientId { get; set; }
        public string AssessmentDescription { get; set; }
    }
}
