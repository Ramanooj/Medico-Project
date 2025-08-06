namespace MedicoAPI.Models
{
    public class PatientAssessment
    {
        public string AssessmentId { get; set; } = Guid.NewGuid().ToString();
        public string PatientId { get; set; }
        public string DoctorId { get; set; }
        public string AssessmentDescription { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }

    }
}
