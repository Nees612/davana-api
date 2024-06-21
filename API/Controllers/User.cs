using System.Text.Json;
using API.Data.Repositories.Interfaces;
using API.Entities;
using API.Services.Authentication;
using API.Services.Authentication.Models;
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
        private readonly UserAuthenticationService _userAuthenticationService;
        private readonly IUsersDynamoRepository _usersDynamoRepository;

        public UserController(ILogger<UserController> logger, IUsersDynamoRepository usersDynamoRepository, UserAuthenticationService userAuthenticationService)
        {
            _usersDynamoRepository = usersDynamoRepository;
            _userAuthenticationService = userAuthenticationService;
            _logger = logger;
        }

        // [HttpGet("all")]
        // public async Task<ActionResult<List<User>>> GetUsers()
        // {
        //     return new OkObjectResult(await _usersDynamoRepository.GetUsers());
        // }

        [HttpGet("isTokenValid")]
        public async Task<ActionResult> IsTokenValid()
        {
            return await Task.Run(() => { return new OkObjectResult(true); });
        }

        [HttpGet("user/{userID}")]
        public async Task<ActionResult<User>> GetUser(string userID)
        {
            if (userID == string.Empty)
                return new BadRequestObjectResult("UserID cannot be null");

            var result = await _usersDynamoRepository.GetUser(userID);

            if (result.Id == string.Empty)
                return new BadRequestObjectResult("Something went wrong");

            return new OkObjectResult(result);
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<ActionResult> CreateUser([FromBody] User user)
        {
            if (user == null)
                return new BadRequestObjectResult("User cannot be null");

            try
            {
                var result = await _usersDynamoRepository.PutUser(user);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Something went wrong {ex.Message}");
            }

            return new OkObjectResult("Coach created successfully!");

        }

        // [AllowAnonymous]
        // [HttpGet("verify-email/{emailHash}")]
        // public async Task<ActionResult> VerifyUserEmail(string emailHash)
        // {
        //     if (emailHash == null)
        //         return new BadRequestObjectResult("Something went wrong");

        //     var result = await _usersDynamoRepository.GetUserByEmailHash(emailHash);

        //     if (result == null)
        //         return new BadRequestObjectResult("Something went wrong");

        //     result.EmailVerified = 1;

        //     return await _usersDynamoRepository.Update(result);

        // }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] Credentials credentials)
        {
            if (credentials == null)
                return new BadRequestObjectResult("Credentials cannot be null");

            var result = await _usersDynamoRepository.CheckUserCredentials(credentials);

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