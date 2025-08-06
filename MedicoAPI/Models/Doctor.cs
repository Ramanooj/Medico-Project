namespace MedicoAPI.Models
{
    public class Doctor
    {
        public string DoctorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DoctorPhone { get; set; }
        public string DoctorCPSONum { get; set; }
        public string ClinicAddress { get; set; }
        public string Email { get; set; }
        public string Specialty { get; set; }
        public bool IsVerified { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<Prescription> Prescriptions { get; set; }
        public ICollection<PatientAssessment> Assessments { get; set; }
    }
}
