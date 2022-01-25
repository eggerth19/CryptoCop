using System.Linq;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;

        public AccountController(IAccountService accountService, ITokenService tokenService)
        {
            _accountService = accountService;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public IActionResult RegisterAccount([FromBody] RegisterInputModel register)
        {
            if (!ModelState.IsValid) { return StatusCode(412, "Invalid registration"); }
            var user = _accountService.CreateUser(register);
            var token = _tokenService.GenerateJwtToken(user);
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("signin")]   
        public IActionResult AccountSignIn([FromBody] LoginInputModel login)
        {
            var user = _accountService.AuthenticateUser(login);
            if (user == null) { return Unauthorized(); }
            var token = _tokenService.GenerateJwtToken(user);
            return Ok(token);
        }

        [HttpGet]
        [Route("signout")]
        public IActionResult AccountSignOut()
        {
            int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "tokenId").Value, out var tokenId);
            _accountService.Logout(tokenId);
            return NoContent();
        }
    }
}