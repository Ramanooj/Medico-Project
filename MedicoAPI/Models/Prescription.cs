namespace MedicoAPI.Models
{
    public class Prescription
    {
        public string PrescriptionId { get; set; } = System.Guid.NewGuid().ToString();
        public string DoctorId { get; set; }
        public string PatientId { get; set; }
        public string PrescriptionContent { get; set; }
        public int RepeatNum { get; set; }
        public int DaysApart {  get; set; }
        public DateTime IssueDate { get; set; } = DateTime.Now;
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
    }
}
