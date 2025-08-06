using System.ComponentModel.DataAnnotations;

namespace MedicoAPI.Models.DTO.Doctor
{
    public class DoctorProfileDTO
    {

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Last name must be up to 100 characters long.")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Last name must be up to 100 characters long.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "The Phone number field is not a valid phone number.")]
        public string DoctorPhone { get; set; }

        [Required(ErrorMessage = "Clinic address is needed")]
        public string ClinicAddress { get; set; }

        public string Specialty { get; set; }

        public string DoctorCPSONum { get; set; }

    }
}
