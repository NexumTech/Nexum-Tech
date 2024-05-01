using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexumTech.Domain.Interfaces;
using NexumTech.Infra.API.Interfaces;
using NexumTech.Infra.Models;

namespace NexumTech.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public LoginController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Authenticate(LoginViewModel loginViewModel)
        {
            try
            {
                var user = await _userService.GetUserInfo(loginViewModel);

                var token = _tokenService.GenerateToken(user);

                return token;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
