using API.Data.Interfaces;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize("coach,user")]
    public class AppointmentController : Controller
    {
        private readonly ILogger<AppointmentController> _logger;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IUserRepository _userRepository;
        public AppointmentController(ILogger<AppointmentController> logger, IAppointmentRepository appointmentRepository, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _logger = logger;
            _appointmentRepository = appointmentRepository;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Appointment>>> GetAppointmens()
        {
            return new OkObjectResult(await _appointmentRepository.GetAppointments());
        }

        [AllowAnonymous]
        [HttpGet("coach-appointments/{coachID}")]
        public async Task<ActionResult<List<Appointment>>> GetAppointmentsByCoachID(int coachID)
        {
            return new OkObjectResult(await _appointmentRepository.GetAppointmentsByCoachID(coachID));
        }

        [Authorize("coach")]
        [HttpPost("appointment-create")]
        public async Task<ActionResult> CreateAppointment([FromBody] Appointment appointment)
        {
            if (appointment == null)
                return new BadRequestObjectResult("Appointment cannot be null.");

            var result = await _appointmentRepository.Add(appointment);

            if (result.GetType() == typeof(OkObjectResult))
                await _appointmentRepository.SaveChangesAsync();

            return result;
        }

        [AllowAnonymous]
        [HttpPost("confirm-booking/{appointmentIDHash}/{userIDHash}")]
        // Needs authorization, confirmation link is sent via email to user,
        // link containes a special token and navigates to this route sending the appointment 
        // This Endpoint can be moved to a other controller "BookingController"
        public async Task<ActionResult> ConfirmBooking(string appointmentIDHash, string userIDHash)
        {
            if (appointmentIDHash == null || userIDHash == null)
                return new BadRequestObjectResult("Something went wrong");

            var result = await _appointmentRepository.GetAppointmentsByIDHash(appointmentIDHash);
            if (result.Id == 0)
                return new BadRequestObjectResult("Booking Failed");
            
            var userID = await _userRepository.GetUserIDByIDHash(userIDHash);
            if (userID == 0)
                return new BadRequestObjectResult("Booking Failed");
            
            result.UserId = userID;
            result.LastUpdatedOn = DateTime.UtcNow;
            await _appointmentRepository.Update(result);
            await _appointmentRepository.SaveChangesAsync();

            //Send notif to Coach -- Send notif to user for waiting approoval from coach
            //With Sendlayer EMAIL service

            return new OkObjectResult("Succefully booked");

        }

        [Authorize("coach")]
        [HttpDelete("appointment-delete")]
        public async Task<ActionResult> DeleteAppointment([FromBody] Appointment appointment)
        {
            if (appointment == null)
                return new BadRequestObjectResult("Appointment cannot be null.");

            var result = await _appointmentRepository.Remove(appointment);

            if (result.GetType() == typeof(OkObjectResult))
                await _appointmentRepository.SaveChangesAsync();

            return result;
        }

    }
}