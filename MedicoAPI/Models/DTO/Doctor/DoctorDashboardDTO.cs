using MedicoAPI.Models.DTO.Appointment;
using MedicoAPI.Models.DTO.Patient;

namespace MedicoAPI.Models.DTO.Doctor
{
    public class DoctorDashboardDTO
    {
        public List<AppointmentDoctorDashboardDTO> Appointments { get; set; } = new();
        public List<PatientProfileDTO> Patients { get; set; } = new();
    }

    public class AppointmentDoctorDashboardDTO 
    {
        public string AppointmentId { get; set; }
        public string Reason { get; set; }
        public DateOnly AppmntDate { get; set; }
        public TimeOnly AppmntTime { get; set; }
        public string PatientFullName { get; set; }
    }


}
