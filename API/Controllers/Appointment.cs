using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
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
        private readonly IAppointmentDynamoRepository _appointmentDynamoRepository;

        public AppointmentController(ILogger<AppointmentController> logger, IAppointmentDynamoRepository appointmentDynamoRepository)
        {
            _appointmentDynamoRepository = appointmentDynamoRepository;
            _logger = logger;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Appointment>>> GetAppointmens()
        {
            return new OkObjectResult(await _appointmentDynamoRepository.GetAppointments());
        }

        [AllowAnonymous]
        [HttpGet("coach-appointments/{coachID}")]
        public async Task<ActionResult<List<Appointment>>> GetAppointmentsByCoachID(string coachID)
        {
            return new OkObjectResult(await _appointmentDynamoRepository.GetAppointmentsByCoachID(coachID));
        }

        [Authorize("coach")]
        [HttpPost("appointment-create")]
        public async Task<ActionResult> CreateAppointment([FromBody] Appointment appointment)
        {
            if (appointment == null)
                return new BadRequestObjectResult("Appointment cannot be null.");

            try
            {
                var result = await _appointmentDynamoRepository.PutAppointment(appointment);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Something went wrong {ex.Message}");
            }

            return new OkObjectResult("Succesfully saved");
        }

        [Authorize("coach")]
        [HttpDelete("appointment-delete")]
        public async Task<ActionResult> DeleteAppointment([FromBody] Appointment appointment)
        {
            if (appointment == null)
                return new BadRequestObjectResult("Appointment cannot be null.");

            try
            {
                await _appointmentDynamoRepository.DeleteByIdAsync(appointment);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Something went wrong {ex.Message}");
            }

            return new OkObjectResult("Succesfully removed");
        }

    }

}