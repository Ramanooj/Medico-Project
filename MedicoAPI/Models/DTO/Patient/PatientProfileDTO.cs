using System.ComponentModel.DataAnnotations;

namespace MedicoAPI.Models.DTO.Patient
{
    // A PATIENT DTO THAT REMOVES THE NAVIGATIONAL PROPERTIES
    public class PatientProfileDTO
    {
        public string PatientId { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be up to 100 characters long.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be up to 100 characters long.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "The Phone number field is not a valid phone number.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Health Card Number is required.")]
        public string HealthCardNumber { get; set; }

        [Required(ErrorMessage = "gender is required.")]
        public char gender { get; set; }
        [Required(ErrorMessage = "PatientAddress is required.")]
        public string PatientAddress { get; set; }

        public ICollection<PatientProfileAllergyDTO> Allergies { get; set; }
        public ICollection<PatientProfileMedsDTO> Medications { get; set; }
    }

    public class PatientProfileAllergyDTO
    {
        public string AllergyId { get; set; }
        public string AllergyName { get; set; }
    }

    public class PatientProfileMedsDTO
    {
        public string MedicationId { get; set; }
        public string MedicationDescription { get; set; }
    }
}