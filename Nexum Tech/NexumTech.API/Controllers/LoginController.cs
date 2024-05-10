using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexumTech.Domain.Interfaces;
using NexumTech.Infra.API.Interfaces;
using NexumTech.Infra.Models;
using System.Globalization;

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

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Authenticate(LoginViewModel loginViewModel)
        {
            try
            {           
                CultureInfo.CurrentCulture = new CultureInfo("pt-BR");

                var user = await _userService.GetUserInfo(loginViewModel);

                var token = _tokenService.GenerateToken(user);

                return token;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> RequestToChangePassword(LoginViewModel loginViewModel)
        {
            try
            {
                await _userService.RequestToChangePassword(loginViewModel.Email);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<string>> AuthenticateWithGoogle(LoginViewModel loginViewModel)
        {
            try
            {
                try
                {
                    await _userService.GetUserInfo(loginViewModel.Email);
                }
                catch (Exception)
                {
                    SignupViewModel signupViewModel = new SignupViewModel
                    {
                        Email = loginViewModel.Email,
                        Username = loginViewModel.Username,
                        PhotoURL = loginViewModel.Photo,
                    };

                    await _userService.CreateUser(signupViewModel);
                }

                var user =  await _userService.GetUserInfo(loginViewModel.Email);

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
