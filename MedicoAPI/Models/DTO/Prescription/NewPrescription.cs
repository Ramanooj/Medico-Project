using System.ComponentModel.DataAnnotations;

namespace MedicoAPI.Models.DTO.Prescription
{
    public class NewPrescription
    {
        [Required(ErrorMessage = "Patient ID is Required")]
        public string PatientId { get; set; }

        [Required(ErrorMessage = "Prescription Instruction is Required")]
        public string PrescriptionContent { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Repeat Number must not be less than 0")]
        public int RepeatNum { get; set; }

        [Required(ErrorMessage = "Dosage Instruction is Required")]
        public int DaysApart { get; set; }

    }
}
