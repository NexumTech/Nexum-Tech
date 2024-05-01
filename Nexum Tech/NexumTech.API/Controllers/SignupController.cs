using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexumTech.Domain.Interfaces;
using NexumTech.Infra.Models;

namespace NexumTech.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SignupController : Controller
    {
        private readonly IUserService _userService;

        public SignupController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Signup(SignupViewModel signupViewModel)
        {
            try
            {
                await _userService.CreateUser(signupViewModel);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
