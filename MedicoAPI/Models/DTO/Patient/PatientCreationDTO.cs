using System.ComponentModel.DataAnnotations;

namespace MedicoAPI.Models.DTO.Patient
{
    public class PatientCreationDTO
    {
        [Required(ErrorMessage = "Gender selection is required.")]
        public char gender { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Health Card Number is required.")]
        //[RegularExpression(@"^[A-Za-z0-9]{10,12}$", ErrorMessage = "Health Card Number must be between 10 to 12 alphanumeric characters.")]
        public string HealthCardNumber { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string ptFullAddress {  get; set; }

        public ICollection<AllergyDTO> Allergies { get; set; }
        public ICollection<MedicationDTO> Medications { get; set; }
    }
    public class AllergyDTO
    {
        public string AllergyName { get; set; }
    }

    public class MedicationDTO
    {
        public string MedicationDescription { get; set; }
    }
}
