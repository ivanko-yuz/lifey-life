using System.IdentityModel.Tokens.Jwt;
using LifeyLife.Core.Contracts.Authentication;
using LifeyLife.Core.Models;
using LifeyLife.Core.Models.Auth;
using LifeyLife.Core.Models.Authentication;
using LifeyLife.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LifeyLife.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsService _accountsService; 
        private readonly JwtHandler _jwtHandler;
    
        public AccountsController(IAccountsService accountsService, JwtHandler jwtHandler) 
        {
            _accountsService = accountsService;
            _jwtHandler = jwtHandler;
        }
    
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] AuthenticationUser userForAuthentication)
        {
            var user = await _accountsService.FindByName(userForAuthentication.Email);
            if (user == null || !await _accountsService.CheckPassword(user, userForAuthentication.Password))
                return Unauthorized(new AuthenticationResponse { ErrorMessage = "Invalid Authentication" });
            var signingCredentials = _jwtHandler.GetSigningCredentials();
            var claims = _jwtHandler.GetClaims(user);
            var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return Ok(new AuthenticationResponse { IsAuthSuccessful = true, Token = token });
        }
    
        [HttpPost] 
        [Route("Registration")]
        public async Task<IActionResult> RegisterUser([FromBody] RegistrationUser userForRegistration) 
        {
            if (userForRegistration == null || !ModelState.IsValid) 
                return BadRequest(); 
            
            var user = new User()
            {
                FirstName = userForRegistration.FirstName,
                LastName = userForRegistration.LastName,
                Email = userForRegistration.Email,
                Password = userForRegistration.Password,
                ConfirmPassword = userForRegistration.ConfirmPassword
            };

            var result = await _accountsService.CreateUser(user, userForRegistration.Password); 
            if (!result) 
            {

                return BadRequest(new RegistrationResponse { Errors = new []{ "Something went wrong;"} }); 
            }
            
            return StatusCode(201); 
        }
    }
}