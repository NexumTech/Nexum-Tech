using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexumTech.Domain.Interfaces;
using NexumTech.Infra.Models;
using System.Security.Claims;

namespace NexumTech.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetUserInfo()
        {
            try
            {
                ClaimsIdentity claims = (ClaimsIdentity)User.Identity;
                string? email = claims?.FindFirst(ClaimTypes.Email)?.Value;

                UserViewModel user = await _userService.GetUserInfo(email);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
