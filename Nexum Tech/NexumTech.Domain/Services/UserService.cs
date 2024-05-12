using Microsoft.Extensions.Localization;
using NexumTech.Domain.Enums;
using NexumTech.Domain.Interfaces;
using NexumTech.Infra.API.Interfaces;
using NexumTech.Infra.DAO.Interfaces;
using NexumTech.Infra.Models;
using System.Globalization;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;

namespace NexumTech.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserDAO _userDAO;
        private readonly IMailService _mailService;
        private readonly IMailMessageService _mailMessageService;
        private readonly IStringLocalizer<UserService> _localizer;

        public UserService(IUserDAO userDAO, IMailService mailService, IMailMessageService mailMessageService, IStringLocalizer<UserService> localizer)
        {
            _userDAO = userDAO;
            _mailService = mailService;
            _mailMessageService = mailMessageService;
            _localizer = localizer;
        }

        public async Task<UserViewModel> GetUserInfo(LoginViewModel loginViewModel)
        {
            var user = await _userDAO.GetUserInfo(loginViewModel);

            if (user == null) throw new Exception(_localizer["InvalidCredentials"]);

            return user;
        }

        public async Task<UserViewModel> GetUserInfo(string email)
        {
            var user = await _userDAO.GetUserInfo(email);

            if (user == null) throw new Exception(_localizer["UserNotFound"]);

            return user;
        }

        public async Task<bool> CreateUser(SignupViewModel signupViewModel)
        {

            bool userExists = await _userDAO.CheckUserExists(signupViewModel.Email);

            if (userExists) throw new Exception(_localizer["UserExists"]);

            if (!String.IsNullOrEmpty(signupViewModel.PhotoURL))
                signupViewModel.Photo = await DownloadImage(signupViewModel.PhotoURL);

            UserViewModel user = new UserViewModel { 
                Email = signupViewModel.Email,
                Username = signupViewModel.Username,     
                Role = "User"
            };

            await _userDAO.CreateUser(signupViewModel);         

            return await SendMailAsync(user, $"{_localizer["Welcome"]}, {signupViewModel.Username}!", MailTypeEnum.Register);
        }

        public async Task<bool> UpdateProfile(ProfileViewModel profileViewModel)
        {
            Regex regex = new Regex(@"^data:image/(jpeg|png);base64,", RegexOptions.IgnoreCase);

            string base64 = regex.Replace(profileViewModel.Base64Photo, "");

            byte[] photo = Convert.FromBase64String(base64);

            return await _userDAO.UpdateUser(profileViewModel.User.Id, profileViewModel.User.Username, photo);
        }

        public async Task<bool> UpdatePassword(string password, string email)
        {
            return await _userDAO.UpdatePassword(password, email);
        }

        public async Task<bool> RequestToChangePassword(string email)
        {

            var user = await _userDAO.GetUserInfo(email);

            if (user == null) throw new Exception(_localizer["UserNotFound"]);

            return await SendMailAsync(user, _localizer["PasswordRecovery"], MailTypeEnum.PasswordReset);
        }

        private async Task<bool> SendMailAsync(UserViewModel user, string subject, MailTypeEnum mailType)
        {
            try
            {
                #region Send e-mail async thread
                    #pragma warning disable CS4014
                        Task.Run(async () =>
                        {
                            await _mailService.SendMail(user.Email, subject, _mailMessageService.GetMailMessage(mailType, user));
                        });
                    #pragma warning restore CS4014
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static async Task<byte[]> DownloadImage(string imageUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(imageUrl);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsByteArrayAsync();
            }
        }
    }
}
