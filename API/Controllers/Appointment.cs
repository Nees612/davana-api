using API.Data.Repositories.Interfaces;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : Controller
    {
        private readonly ILogger<AppointmentController> _logger;
        private readonly IAppointmentRepository _appointmentRepository;
        public AppointmentController(ILogger<AppointmentController> logger, IAppointmentRepository appointmentRepository)
        {
            _logger = logger;
            _appointmentRepository = appointmentRepository;
        }

        [AllowAnonymous]
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