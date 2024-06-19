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
        private readonly ICoachRepository _coachRepository;
        private readonly CoachAuthenticationService _coachAuthenticationService;
        private readonly ICoachesDynamoRepository _coachesDynamoRepository;

        public CoachController(ILogger<CoachController> logger, ICoachRepository coachRepository, CoachAuthenticationService coachAuthenticationService, ICoachesDynamoRepository coachesDynamoRepository)
        {
            _coachesDynamoRepository = coachesDynamoRepository;
            _coachAuthenticationService = coachAuthenticationService;
            _coachRepository = coachRepository;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<ActionResult<List<CoachDTO>>> GetCoaches()
        {
            return new OkObjectResult(await _coachRepository.GetCoaches());
        }

        [AllowAnonymous]
        [HttpGet("coach/{coachID}")]
        public async Task<ActionResult<CoachDTO>> GetCoach(int coachID)
        {
            if (coachID == 0)
                return new BadRequestObjectResult("Coach ID cannot be 0");


            // var coach = await _coachRepository.GetCoach(coachID);
            var coach = await _coachesDynamoRepository.GetByIdAsync(coachID.ToString());

            if (coach.Id == 0)
                return new BadRequestObjectResult("Something went wrong");

            return new OkObjectResult(coach);

        }

        [AllowAnonymous]
        [HttpPost("coach-create")]
        public async Task<ActionResult> CreateCoach([FromBody] Coach coach)
        {
            if (coach == null)
                return new BadRequestObjectResult("Coach cannot be null");

            var result = await _coachRepository.Add(coach);

            _coachesDynamoRepository.PutItem(coach);

            if (result.GetType() == typeof(OkObjectResult))
                await _coachRepository.SaveChangesAsync();

            return result;
        }

        [HttpPost("coach-update")]
        public async Task<ActionResult> UpdateCoach([FromBody] Coach coach)
        {
            if (coach == null)
                return new BadRequestObjectResult("Coach cannot be null");

            var result = await _coachRepository.Update(coach);

            if (result.GetType() == typeof(OkObjectResult))
                await _coachRepository.SaveChangesAsync();

            return result;
        }

        [HttpDelete("coach-delete")]
        public async Task<ActionResult> DeleteCoach([FromBody] Coach coach)
        {
            if (coach == null)
                return new BadRequestObjectResult("Coach cannot be null");

            var result = await _coachRepository.Remove(coach);

            if (result.GetType() == typeof(OkObjectResult))
                await _coachRepository.SaveChangesAsync();

            return result;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] Credentials credentials)
        {
            if (credentials == null)
                return new BadRequestObjectResult("Credentials cannot be empty");

            //password needs to be hashed - will be hashed on Client Side           
            var coach = await _coachRepository.CheckCoachCredentials(credentials);

            if (coach.Id == 0)
                return new BadRequestObjectResult("Something went Wrong");

            var tokenresult = await _coachAuthenticationService.GenerateToken(coach);

            if (tokenresult == null)
                return new BadRequestObjectResult("Something went wrong");

            return new OkObjectResult(tokenresult);

        }

    }
}