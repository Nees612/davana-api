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
    public class UserController(ILogger<UserController> logger, IUsersDynamoRepository usersDynamoRepository) : Controller
    {
        private readonly ILogger<UserController> _logger = logger;
        private readonly IUsersDynamoRepository _usersDynamoRepository = usersDynamoRepository;

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

            if (result == null)
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

        [AllowAnonymous]
        [HttpGet("verify-email/{userID}")]
        public async Task<ActionResult> VerifyUserEmail(string userID)
        {
            if (userID == null)
                return new BadRequestObjectResult("Something went wrong");

            var result = await _usersDynamoRepository.GetUser(userID);

            if (result == null)
                return new BadRequestObjectResult("Something went wrong");

            result.EmailVerified = 1;

            await _usersDynamoRepository.SaveAsync(result);

            return new OkObjectResult($"User: {userID}\n Email verification is successful");

        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] Credentials credentials)
        {
            if (credentials == null)
                return new BadRequestObjectResult("Credentials cannot be null");

            var result = await _usersDynamoRepository.CheckUserCredentials(credentials);

            if (result == null)
                return new BadRequestObjectResult("Something went wrong");

            var tokenresult = await UserAuthenticationService.GenerateToken(result);

            if (tokenresult == null)
                return new BadRequestObjectResult("Something wnet wrong");

            var tokenAsJson = JsonSerializer.Serialize(tokenresult);
            return new OkObjectResult(tokenAsJson);
        }

    }
}