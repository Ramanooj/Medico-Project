using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicoAPI.Data;
using MedicoAPI.Models;
using MedicoAPI.Models.DTO.Appointment;
using Microsoft.IdentityModel.Tokens;
using MedicoAPI.Models.DTO.Doctor;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;

// ONLY ALLOW PATIENTS THAT HAVE REGISTERED TO BOOK AN APPOINTMENT
// MAKE SURE THAT AN ADDRESS IS ONLY FROM ONTARIO OR ELSE IT WILL CRASH

namespace MedicoAPI.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]

    public class AppointmentsController : ControllerBase
    {
        private readonly MedicoAPIContext _context;
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        private readonly AppointmentService _appointmentReminder;


        public AppointmentsController(MedicoAPIContext context, IConfiguration config, HttpClient httpClient, AppointmentService appointmentReminder)
        {
            _context = context;
            _config = config;
            _httpClient = httpClient;
            _appointmentReminder = appointmentReminder;

        }

        [Authorize(Roles = "patient")]
        [HttpPost("Create")]
        public async Task<ActionResult> PostAppointment(AppointmentDTOComplete newAppointment)
        {
            if (!PatientExists())
            {
                ModelState.AddModelError("Error", "Patient not registered");
                return BadRequest(ModelState);
            }
            if (!isDoctorVerified(newAppointment.DoctorID))
            {
                ModelState.AddModelError("Error", "Unverified doctor");
                return BadRequest(ModelState);
            }
            if (ModelState.IsValid)
            {
                if (await isDoctorAvailable(newAppointment.DoctorID, newAppointment.AppmntDate, newAppointment.AppmntTime))
                {
                    var appointment = new Appointment
                    {
                        PatientId = getPatientId(),
                        DoctorId = newAppointment.DoctorID,
                        Reason = newAppointment.Reason,
                        AppmntDate = newAppointment.AppmntDate,
                        AppmntTime = newAppointment.AppmntTime
                    };

                    var ptPhoneNum = await _context.Patient.AsNoTracking().Where(pt => pt.PatientId == getPatientId()).Select(pt => pt.PhoneNumber).FirstOrDefaultAsync();
                    var apptId = appointment.AppointmentId;
                    var apptTime = appointment.AppmntTime;
                    var whenToRemind = appointment.AppmntDate.ToDateTime(apptTime).AddMinutes(-30);

                    if (newAppointment.IsToBeNotified)
                    {
                       await _appointmentReminder.CreateOneTimeReminder(apptId, whenToRemind, ptPhoneNum, apptTime.ToString());
                    }

                    _context.Appointment.Add(appointment);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                ModelState.AddModelError("Error", "Doctor is not available on the selected time slot. Please select another one");
                return BadRequest(ModelState);
            }
            ModelState.AddModelError("Error", "Invalid parameters");
            return BadRequest(ModelState);
        }

        [Authorize(Roles = "patient, doctor")]
        [HttpGet("VideoCallAppointments")]
        public async Task<ActionResult<List<VideoCallAppointmentDTO>>> GetAllAppointments()
        {
            var userId = getPatientId();
            var role = User.FindFirst("cognito:groups")?.Value;

            if (role.Contains("patient"))
            {

                var appointments = await _context.Appointment
                                 .AsNoTracking()
                                 .Where(app => app.PatientId == userId)
                                 .Include(app => app.Doctor)
                                 .ToListAsync();

                var patientAppointments = appointments
                                     .Where(app => app.AppmntDate.ToDateTime(app.AppmntTime) > DateTime.Now)
                                     .Select(app => new VideoCallAppointmentDTO
                                     {
                                         AppointmentId = app.AppointmentId,
                                         CounterpartName = $"Dr {app.Doctor.FirstName} {app.Doctor.LastName}",
                                         AppointmentSchedule = app.AppmntDate.ToDateTime(app.AppmntTime)
                                     }).ToList();

                return Ok(patientAppointments);
            }
            if (role.Contains("doctor"))
            {

                var appointments = await _context.Appointment
                                 .AsNoTracking()
                                 .Where(app => app.DoctorId == userId)
                                 .Include(app => app.Patient)
                                 .ToListAsync();

                var doctorAppointments = appointments
                                    .Where(app => app.AppmntDate.ToDateTime(app.AppmntTime) > DateTime.Now)
                                    .Select(app => new VideoCallAppointmentDTO
                                    {
                                        AppointmentId = app.AppointmentId,
                                        CounterpartName = $"{app.Patient.FirstName} {app.Patient.LastName}",
                                        AppointmentSchedule = app.AppmntDate.ToDateTime(app.AppmntTime)
                                    })
                                    .ToList();

                return Ok(doctorAppointments);
            }

            ModelState.AddModelError("Error", "User not Registered");
            return BadRequest(ModelState);
        }

        [Authorize(Roles = "patient")]
        [HttpGet("Proximity")]
        public async Task<ActionResult<List<DoctorProximity>>> DoctorsInTheVicinity([FromQuery] string patientAddress = "", [FromQuery] int distanceInKM = 5)
        {
            if(!PatientExists())
            {
                ModelState.AddModelError("Error", "Patient not registered");
                return BadRequest(ModelState);
            }
            if (!string.IsNullOrWhiteSpace(patientAddress))
            {
                //return the doctors in the vicinity of the address with order of closest to farthest
                return Ok(await ProximityCalculator(patientAddress, distanceInKM));
            }
            else {
                var ptAddress = await _context.Patient.AsNoTracking()
                                    .Where(patient => patient.PatientId == getPatientId())
                                    .Select(pt => pt.PatientAddress)
                                    .FirstAsync();

                // call the function that evaluates the closest doctor in the vicinity of an address
                return Ok(await ProximityCalculator(ptAddress, distanceInKM));
            }
        }

        // CHECKS AVAILABLE TIMESLOTS OF A SELECTED DOCTOR FOR A PERIOD OF TIME
        [Authorize(Roles = "patient")]
        [HttpGet("Availability")]
        public async Task<ActionResult<Dictionary<DateOnly, List<TimeOnly>>>> GetDoctorAvailability([FromQuery] string doctorId)
        {
            var doctorExists = _context.Doctor.Any(docs => docs.DoctorId == doctorId);

            if (!string.IsNullOrEmpty(doctorId) && doctorExists)
            {
                var DocSched2Weeks = await DoctorTimeslots(doctorId);

                if (DocSched2Weeks != null)
                {
                    return Ok(DocSched2Weeks);
                }
            }
            ModelState.AddModelError("Error", "Doctor may not exists");
            return BadRequest(ModelState);
        }

        [Authorize(Roles = "patient")]
        [HttpDelete("Cancel")]
        public async Task<ActionResult> DeleteAppointment([FromQuery] string appointmentId)
        {
            var appointment = await _context.Appointment.FindAsync(appointmentId);

            if (appointment == null || appointment.PatientId != getPatientId())
            {
                ModelState.AddModelError("Error", "Appointment does not exists");
                return BadRequest(ModelState);
            }

            var appointmentDateTime = appointment.AppmntDate.ToDateTime(appointment.AppmntTime);

            if ((DateTime.Now - appointment.AppointmenCreation).TotalHours <= 2)
            {
                _context.Appointment.Remove(appointment);
                await _context.SaveChangesAsync();
                return Ok();
            }

            if ((appointmentDateTime - DateTime.Now).TotalHours < 24)
            {
                ModelState.AddModelError("Error", "Cancelling an appointment 24 hours prior is not allowed");
                return BadRequest(ModelState);
            }

            _context.Appointment.Remove(appointment);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //CHECKS WHETHER THE DOCTOR IS AVAIALBLE 
        //USAGE - SECONDARY CHECK AFTER AN APPOINTMENT CONFIRMED TO AVOID DUPLICATES IF AN 'INSERT-RACE' WERE TO HAPPEN.
        private async Task<bool> isDoctorAvailable(string doctorID, DateOnly date, TimeOnly time)
        {
            var doctorTimeSlots = await DoctorTimeslots(doctorID);
            if (doctorTimeSlots != null)
            {
                if (doctorTimeSlots.ContainsKey(date) && doctorTimeSlots[date].Contains(time))
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        private async Task<Dictionary<DateOnly, List<TimeOnly>>> DoctorTimeslots(string doctorID)
        {
            //THE PERIOD OF TIME IN WHICH THE TIMESLOTS ARE CHECKED
            var todaysDate = DateOnly.FromDateTime(DateTime.Today);
            var endDate = todaysDate.AddDays(14);

            //DOCTOR WORKING HOURS
            //REFACTOR THIS TO DYNAMICALLY SET THE WORKING HOURS OF A DOCTOR
            var startOfWorkingDay = new TimeOnly(9, 0);
            var endOfWorkingDay = new TimeOnly(17, 0);

            //RETRIEVES ALL THE BOOKED APPOINTMENTS OF A DOCTOR
            var bookedAppointments = await _context.Appointment.AsNoTracking()
                                                   .Where(appmnt => appmnt.DoctorId == doctorID && appmnt.AppmntDate >= todaysDate && appmnt.AppmntDate <= endDate)
                                                   .ToListAsync();

            //CREATES A DICTIONARY WITH ITEMS IN A PAIR OF [DAY (key) - WORKING HOURS / LIST OF TIMESLOTS] 
            var timeslot = new Dictionary<DateOnly, List<TimeOnly>>();

            //LOOPS FROM THE STARTING / PRESESNT DAY UNTIL END DATE AND INCREMENT BY ONE DAY
            for (var date = todaysDate; date <= endDate; date = date.AddDays(1))
            {
                //CREATES A TIMESLOT LIST FROM 9AM - 5PM (WORKING HOURS) AND POPULATE IT IN A 30 MINS INTERVAL
                var dailySlots = new List<TimeOnly>();
                var currentTime = startOfWorkingDay;

                if (date == todaysDate)
                {
                    var now = TimeOnly.FromDateTime(DateTime.Now);
                    // If the current time is later than the start of the working day, use the current time
                    if (now > startOfWorkingDay)
                    {
                        // if the current time is larger than the end of the working day, continue to the next day
                        if(now > startOfWorkingDay)
                        {
                            continue;
                        }
                        currentTime = now.AddMinutes(30 - now.Minute % 30);
                    }
                }

                while (currentTime < endOfWorkingDay)
                {
                    var currentTimeAdjusted = new TimeOnly(currentTime.Hour, currentTime.Minute, 0);
                    dailySlots.Add(currentTimeAdjusted);
                    currentTime = currentTime.AddMinutes(30);
                }

                //INSERTS THE DAY'S TIME SLOTS IN THE DICTIONARY WITH A KEY OF LOOP DATE
                timeslot.Add(date, dailySlots);
            }

            //HAND PICKING EACH TAKEN TIMESLOTS IN THE DICTIONARY BY CROSS CHECKING IT WITH THE SCHEDULED APPOINTMENT
            foreach (var appointment in bookedAppointments)
            {
                if (timeslot.ContainsKey(appointment.AppmntDate))
                {
                    timeslot[appointment.AppmntDate].Remove(appointment.AppmntTime);
                }
            }

            return timeslot;
        }

        private async Task<List<DoctorProximity>> ProximityCalculator(string origin, int distanceInKM)
        {
            // 1. GET ALL DOCTORS STORE THEM INTO AN LIST
            var listOfDoctors = await _context.Doctor.AsNoTracking()
                                .Where(doc => doc.IsVerified)
                                .Select(doctor => new DoctorProximity
                                {
                                    DoctorId = doctor.DoctorId,
                                    DoctorName = $"Dr. {doctor.FirstName} {doctor.LastName}",
                                    Specialty = doctor.Specialty,
                                    ClinicAddress = doctor.ClinicAddress
                                }).ToListAsync();

            // 2. GET ALL THEIR ADDRESSES FROM THE LIST
            var destinations = listOfDoctors.Select(doctor => doctor.ClinicAddress).ToArray();

            // FORMAT THE ADDRESSES INTO GOOGLE API CAN UNDERSTAND
            string destinationsFormatted = string.Join("|", destinations);


            // 3. QUERY GOOGLE DISTANCE MATRIX API SEND ALL DESTINATION ADDRESS ('listOfAddreses') WITH AN ORIGIN OF 'patientAddress'
            var requestUrl = $"https://maps.googleapis.com/maps/api/distancematrix/json?origins={origin}&destinations={destinationsFormatted}&key={_config["GoogleAPIKey"]}";
            var response = await _httpClient.GetStringAsync(requestUrl);

            // 4. DESERIALIZE RESPONSE FROM THE QUERY AND TURN INTO A LIST

            // ERROR HERE IN THE DEPLOYED API
            var deserialized = JObject.Parse(response);
            var elements = deserialized["rows"][0]["elements"]
                           .Select(result => new
                           {
                               DistanceInKm = (string) result["distance"]["text"],
                               Status = (string) result["status"]
                           }).ToList();

            // 5. INSERT THE EACH DISTANCE TO THE 'listOfDoctors' (THE ORDER OF THE RESPONSE IS CONSISTENT WITH QUERY)
            //   CHECK IF STATUS IS OK AND INSERT, IF NOT THEN USE 'CONTINUE' KEYWORD TO SKIP
            for (int i = 0; i < listOfDoctors.Count; i++)
            {
                if (elements[i].Status != "OK")
                {
                    continue;
                }
                listOfDoctors[i].EstimatedDistance = elements[i].DistanceInKm ?? "Unavailable";
            }


            // 6. SORT THE LIST AFTER DISTANCE ARE ADDED
            listOfDoctors.OrderBy(doctors => Convert.ToDouble(doctors.EstimatedDistance.Replace(" km", "", StringComparison.OrdinalIgnoreCase)));
            listOfDoctors.RemoveAll(doctors => Convert.ToDouble(doctors.EstimatedDistance.Replace(" km", "", StringComparison.OrdinalIgnoreCase)) > distanceInKM);

            if (listOfDoctors.Count > 15)
            {
                listOfDoctors.RemoveRange(15, listOfDoctors.Count - 15);
            }

            // 7. RETURN LIST
            return listOfDoctors;
        }


        private bool isDoctorVerified(string id)
        {
            return _context.Doctor.Find(id)?.IsVerified ?? false;
        }

        private string getPatientId()
        {
            return User.FindFirst("username").Value;
        }

        private bool PatientExists()
        {
            return _context.Patient.AsNoTracking().Any(e => e.PatientId == getPatientId());
        }

    }
}