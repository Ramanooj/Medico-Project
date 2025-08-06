namespace MedicoAPI.Models
{
    public class Patient
    {
        public string PatientId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public char Gender { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string HealthCardNumber { get; set; }
        public string PatientAddress { get; set; }



        // Navigation properties
        public ICollection<PatientAssessment> Assessments { get; set; }
        public ICollection<Allergy> Allergies { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<Medication> Medications { get; set; }
        public ICollection<Prescription> Prescriptions { get; set; }
       
    }
}
