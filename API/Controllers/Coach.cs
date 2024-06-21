using Amazon.DynamoDBv2.DocumentModel;
using API.Data.DTO;
using API.Data.Repositories.Interfaces;
using API.Entities;
using API.Services.Authentication;
using API.Services.Authentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // This whole api route needs authorization
    [Authorize("coach")]
    public class CoachController : Controller
    {
        private readonly ILogger<CoachController> _logger;
        private readonly CoachAuthenticationService _coachAuthenticationService;
        private readonly ICoachesDynamoRepository _coachesDynamoRepository;

        public CoachController(ILogger<CoachController> logger, CoachAuthenticationService coachAuthenticationService, ICoachesDynamoRepository coachesDynamoRepository)
        {
            _coachesDynamoRepository = coachesDynamoRepository;
            _coachAuthenticationService = coachAuthenticationService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<ActionResult<List<Document>>> GetCoaches()
        {
            return new OkObjectResult(await _coachesDynamoRepository.GetCoaches());
        }

        [AllowAnonymous]
        [HttpGet("coach/{coachID}")]
        public async Task<ActionResult<Coach>> GetCoach(string coachID)
        {
            if (coachID == string.Empty)
                return new BadRequestObjectResult("Coach ID cannot be empty");

            var coach = await _coachesDynamoRepository.GetCoach(coachID);

            if (coach == null)
                return new BadRequestObjectResult("Something went wrong");

            return new OkObjectResult(coach);

        }

        [AllowAnonymous]
        [HttpPost("coach-create")]
        public async Task<ActionResult> CreateCoach([FromBody] Coach coach)
        {
            if (coach == null)
                return new BadRequestObjectResult("Coach cannot be null");

            try
            {
                await _coachesDynamoRepository.PutCoach(coach);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Something went wrong {ex.Message}");
            }

            return new OkObjectResult("Coach created successfully!");
        }

        [HttpPost("coach-update")]
        public async Task<ActionResult> UpdateCoach([FromBody] Coach coach)
        {
            if (coach == null)
                return new BadRequestObjectResult("Coach cannot be null");

            try
            {
                await _coachesDynamoRepository.SaveAsync(coach);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Something went wrong {ex.Message}");
            }

            return new OkObjectResult("Succesfully saved");
        }

        [AllowAnonymous]
        [HttpDelete("coach-delete")]
        public async Task<ActionResult> DeleteCoach([FromBody] Coach coach)
        {
            if (coach == null)
                return new BadRequestObjectResult("Coach cannot be null");

            try
            {
                await _coachesDynamoRepository.DeleteByIdAsync(coach);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Something went wrong {ex.Message}");
            }

            return new OkObjectResult("Succesfully removed");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] Credentials credentials)
        {
            if (credentials == null)
                return new BadRequestObjectResult("Credentials cannot be empty");

            //password needs to be hashed - will be hashed on Client Side           
            var coach = await _coachesDynamoRepository.CheckCoachCredentials(credentials);

            if (coach.Id == string.Empty)
                return new BadRequestObjectResult("Something went Wrong");

            var tokenresult = await _coachAuthenticationService.GenerateToken(coach);

            if (tokenresult == null)
                return new BadRequestObjectResult("Something went wrong");

            return new OkObjectResult(tokenresult);

        }

    }
}