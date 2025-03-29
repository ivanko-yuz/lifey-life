using System.IdentityModel.Tokens.Jwt;
using LifeyLife.Core.Contracts.Authentication;
using LifeyLife.Core.Models;
using LifeyLife.Core.Models.Auth;
using LifeyLife.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace LifeyLife.Api.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsService _accountsService;
        private readonly JwtHandler _jwtHandler;

        public AccountsController(
            IAccountsService accountsService,
            JwtHandler jwtHandler)
        {
            _accountsService = accountsService;
            _jwtHandler = jwtHandler;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationUser registration)
        {
            if (registration.Password != registration.ConfirmPassword)
            {
                return BadRequest(new { Error = "Passwords do not match" });
            }

            var user = new User
            {
                Uuid = Guid.NewGuid(),
                Email = registration.Email
            };

            var result = await _accountsService.CreateUser(user, registration.Password);
            if (!result)
            {
                return BadRequest(new { Error = "Failed to create user" });
            }

            return Ok(new { Message = "User registered successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticationUser login)
        {
            var user = await _accountsService.FindByName(login.Email);
            if (user == null || !await _accountsService.CheckPassword(user, login.Password))
            {
                return Unauthorized(new { Error = "Invalid email or password" });
            }

            var signingCredentials = _jwtHandler.GetSigningCredentials();
            var claims = _jwtHandler.GetClaims(user);
            var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return Ok(new { Token = token });
        }
    }
}