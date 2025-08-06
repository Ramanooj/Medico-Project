using System.ComponentModel.DataAnnotations.Schema;

namespace MedicoAPI.Models
{
    public class MedicalHistory
    {
        public string MedicalHistoryId { get; set; } = System.Guid.NewGuid().ToString();

        [ForeignKey("PatientId")]
        public string PatientId { get; set; }

        [ForeignKey("PatientId")]
        public string DoctorId { get; set; }

        public string Description { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
    }
}
