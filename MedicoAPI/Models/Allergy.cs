namespace MedicoAPI.Models
{
    public class Allergy
    {
        public string AllergyId { get; set; } = System.Guid.NewGuid().ToString();
        public string PatientId { get; set; }
        public string AllergyName { get; set; }

        public Patient Patient { get; set; }
    }
}