using System.ComponentModel.DataAnnotations;

namespace MedicoAPI.Models.DTO.Appointment
{
    public class AppointmentDTOComplete
    {


        [Required(ErrorMessage = "Appointment reasion is required.")]
        public string Reason { get; set; }
        [Required(ErrorMessage = "Appointment Date is required.")]
        public DateOnly AppmntDate { get; set; }
        [Required(ErrorMessage = "Appointment Time is required.")]
        public TimeOnly AppmntTime { get; set; }
        [Required(ErrorMessage = "DoctorID ID is Required")]
        public string DoctorID { get; set; }

        public bool IsToBeNotified { get; set; }
    }
}

/*
            [Range(1, double.MaxValue, ErrorMessage = "Please Enter a valid doctor id")]
 */