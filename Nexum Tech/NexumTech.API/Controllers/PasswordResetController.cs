using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexumTech.Domain.Interfaces;
using NexumTech.Infra.Models;
using System.Security.Claims;

namespace NexumTech.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PasswordResetController : ControllerBase
    {
        private readonly IUserService _userService;

        public PasswordResetController(IUserService userService) 
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult PasswordResetTokenValidation() // Just to validade JWT Token with authorize at password reset
        {
            return Ok();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdatePassword(PasswordResetViewModel passwordResetViewModel) 
        {
            try
            {
                var email = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                await _userService.UpdatePassword(passwordResetViewModel.Password, email);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }
    }
}
