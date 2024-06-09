using System.Text.Json;
using API.Data.Interfaces;
using API.Entities;
using API.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize("user")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly UserAuthenticationService _userAuthenticationService;

        public UserController(ILogger<UserController> logger, IUserRepository userRepository, UserAuthenticationService userAuthenticationService)
        {
            _userAuthenticationService = userAuthenticationService;
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            return new OkObjectResult(await _userRepository.GetUsers());
        }

        [HttpGet("isTokenValid")]
        public async Task<ActionResult> IsTokenValid()
        {
            return await Task.Run(() => { return new OkObjectResult(true); });
        }

        [HttpGet("user/{userID}")]
        public async Task<ActionResult<User>> GetUser(int userID)
        {
            if (userID == 0)
                return new BadRequestObjectResult("UserID cannot be null");

            var result = await _userRepository.GetUser(userID);

            if (result.Id == 0)
                return new BadRequestObjectResult("Something went wrong");

            return new OkObjectResult(result);
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<ActionResult> CreateUser([FromBody] User user)
        {
            if (user == null)
                return new BadRequestObjectResult("User cannot be null");

            var result = await _userRepository.Add(user);

            if (result.GetType() == typeof(OkObjectResult))
                await _userRepository.SaveChangesAsync();

            return result;
        }

        [AllowAnonymous]
        [HttpGet("verify-email/{emailHash}")]
        public async Task<ActionResult> VerifyUserEmail(string emailHash)
        {
            if (emailHash == null)
                return new BadRequestObjectResult("Something went wrong");

            var result = await _userRepository.GetUserByEmailHash(emailHash);

            if (result == null)
                return new BadRequestObjectResult("Something went wrong");

            result.EmailVerified = 1;

            return await _userRepository.Update(result);

        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] Credentials credentials)
        {
            if (credentials == null)
                return new BadRequestObjectResult("Credentials cannot be null");

            var result = await _userRepository.CheckUserCredentials(credentials);

            if (result == null)
                return new BadRequestObjectResult("Something went wrong");

            var tokenresult = await _userAuthenticationService.GenerateToken(result);

            if (tokenresult == null)
                return new BadRequestObjectResult("Something wnet wrong");

            var tokenAsJson = JsonSerializer.Serialize(tokenresult);
            return new OkObjectResult(tokenAsJson);
        }

    }
}