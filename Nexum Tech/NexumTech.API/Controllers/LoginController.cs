using Microsoft.AspNetCore.Mvc;
using Nexum_Tech.Domain.Interfaces;
using NexumTech.Infra.Models;

namespace NexumTech.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogin _loginService;

        public LoginController(ILogin loginService)
        {
            _loginService = loginService;
        }

        [HttpGet]
        public async Task<Object> Authentication(LoginViewModel loginViewModel)
        {
            return _loginService.Authentication(loginViewModel);
        }
    }
}
