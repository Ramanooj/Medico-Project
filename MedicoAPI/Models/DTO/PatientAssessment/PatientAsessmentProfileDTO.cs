using System.ComponentModel.DataAnnotations;

namespace MedicoAPI.Models.DTO.PatientAssessment
{
    public class PatientAsessmentProfileDTO
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string HealthCardNumber { get; set; }
        public char Gender { get; set; }
        public string PatientAddress { get; set; }

        public ICollection<PatientAssessmentProfileAllergyDTO> Allergies { get; set; }
        public ICollection<PatientAssessmentProfileMedsDTO> Medications { get; set; }
    }
    public class PatientAssessmentProfileAllergyDTO
    {
        public string AllergyName { get; set; }
    }

    public class PatientAssessmentProfileMedsDTO
    {
        public string MedicationDescription { get; set; }
    }


}
