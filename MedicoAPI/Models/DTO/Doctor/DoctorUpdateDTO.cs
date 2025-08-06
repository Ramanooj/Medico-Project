using System.ComponentModel.DataAnnotations;

namespace MedicoAPI.Models.DTO.Doctor
{
    public class DoctorUpdateDTO
    {
        [Required(ErrorMessage = "Clinic address is needed")]
        public string ClinicAddress { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Clinic address is needed")]
        public string Specialty { get; set; }
    }
}
