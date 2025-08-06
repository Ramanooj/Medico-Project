namespace MedicoAPI.Models
{
    public class Appointment
    {
        public string AppointmentId { get; set; } = System.Guid.NewGuid().ToString();
        public string PatientId { get; set; }
        public string DoctorId { get; set; }
        public string Reason { get; set; }
        public DateOnly AppmntDate { get; set; }
        public TimeOnly AppmntTime { get; set; }

        public DateTime AppointmenCreation {  get; set; } = DateTime.Now;
        // Navigation properties
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
    }

}
