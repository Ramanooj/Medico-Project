using System.ComponentModel.DataAnnotations;

namespace MedicoAPI.Models.DTO.Patient
{
    public class PatientUpdateDTO
    {

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "PatientAddress is required.")]
        public string PatientAddress { get; set; }

    }
}
