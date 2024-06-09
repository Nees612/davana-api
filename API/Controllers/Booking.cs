using API.Data.Interfaces;
using API.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : Controller
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly UserAuthenticationService _userAuthenticationService;

        public BookingController(ILogger<BookingController> logger, IAppointmentRepository appointmentRepository, IUserRepository userRepository, UserAuthenticationService userAuthenticationService)
        {
            _userAuthenticationService = userAuthenticationService;
            _userRepository = userRepository;
            _appointmentRepository = appointmentRepository;
            _logger = logger;
        }

        [Authorize("user")]
        [HttpPost("start-booking")]
        public async Task<ActionResult> StartBooking([FromBody] int appointmentID)
        {

            if (appointmentID == 0)
                return new BadRequestObjectResult("Something went wrong");

            var result = await _appointmentRepository.GetAppointmentByID(appointmentID);

            if (result.Id == 0)
                return new BadRequestObjectResult("Something went wrong");


            //Send notification to responsible Coach -- TODO 
            //Send notification to User that the booking is in waiting for aprooval from coach


            //In development i send back the whole appointment, with the user id..

            var authorizationHeader = HttpContext.Request.Headers["Authorization"];
            string accessToken = string.Empty;
            if (authorizationHeader.ToString().StartsWith("Bearer"))
            {
                accessToken = authorizationHeader.ToString().Substring("Bearer ".Length).Trim();
            }

            result.UserId = int.Parse(_userAuthenticationService.DecodeToken(accessToken).Claims.First(claim => claim.Type == "funny").Value);
            return new OkObjectResult(result);
        }

        // [AllowAnonymous]
        // [HttpPost("confirm-booking/{appointmentIDHash}/{userIDHash}")]
        // //Needs SendLayer implementation, to send notification
        // public async Task<ActionResult> ConfirmBooking(string appointmentIDHash, string userIDHash)
        // {
        //     if (appointmentIDHash == null || userIDHash == null)
        //         return new BadRequestObjectResult("Something went wrong");

        //     var result = await _appointmentRepository.GetAppointmentsByIDHash(appointmentIDHash);
        //     if (result.Id == 0)
        //         return new BadRequestObjectResult("Something went wrong");

        //     var userID = await _userRepository.GetUserIDByIDHash(userIDHash);
        //     if (userID == 0)
        //         return new BadRequestObjectResult("Something went wrong");

        //     result.UserId = userID;
        //     result.LastUpdatedOn = DateTime.UtcNow;
        //     await _appointmentRepository.Update(result);
        //     await _appointmentRepository.SaveChangesAsync();

        //     //Send notif to Coach -- Send notif to user for waiting approoval from coach
        //     //With Sendlayer EMAIL service 

        //     return new OkObjectResult("Succefully booked");

        // }

    }
}