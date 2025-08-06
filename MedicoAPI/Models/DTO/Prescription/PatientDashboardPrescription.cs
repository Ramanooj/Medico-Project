namespace MedicoAPI.Models.DTO.Prescription
{
    public class PatientDashboardPrescription
    {
        public string PrescriptionId { get; set; }
        public string PrescriptionContent { get; set; }
        public DateTime IssueDate { get; set; }
        public string DoctorName { get; set; }
    }
}
