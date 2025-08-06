namespace MedicoAPI.Models
{
    public class Medication
    {
        public string MedicationId { get; set; } = System.Guid.NewGuid().ToString();
        public string PatientId { get; set; }
        public string MedicationDescription { get; set; }
        public Patient Patient { get; set; }
    }
}