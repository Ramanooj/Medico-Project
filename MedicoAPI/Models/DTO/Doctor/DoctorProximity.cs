using System.ComponentModel.DataAnnotations;

namespace MedicoAPI.Models.DTO.Doctor
{
    public class DoctorProximity
    {
        public string DoctorId { get; set; }

        public string DoctorName { get; set; }

        public string Specialty { get; set; }

        public string ClinicAddress { get; set; }

        public string EstimatedDistance { get; set; }
    }
}

