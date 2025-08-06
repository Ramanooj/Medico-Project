using MedicoAPI.Models.DTO.Prescription;

namespace MedicoAPI.Models.DTO.Patient
{
    public class PatientDashboardDTO
    {
        public List<PatientDashBoardAppointmentDTO> Appointments { get; set; } = new();
        public List<PatientDashboardPrescription> Prescriptions { get; set; } = new();

    }

    public class PatientDashBoardAppointmentDTO
    {
        public string DoctorFullName { get; set; }
        public string AppointmentId { get; set; }
        public string Reason { get; set; }
        public DateTime appointmentSchedule { get; set; }
        public string PatientFullName { get; set; }
    }
}
