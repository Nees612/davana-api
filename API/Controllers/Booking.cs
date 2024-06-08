using API.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : Controller
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IUserRepository _userRepository;

        public BookingController(ILogger<BookingController> logger, IAppointmentRepository appointmentRepository, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _appointmentRepository = appointmentRepository;
            _logger = logger;
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

    }
}