using BookingCare.Application.DTOs.Requests.Account;
using BookingCare.Application.Utils;
using BookingCare.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System.Text;

namespace BookingCare.Application.Services
{
    public interface IAccountService
    {
        Task SendMailResetPassword(SendMailResetPasswordRequest request);
        Task<IdentityResult> ResetPassword(ResetPasswordRequest request);
        Task<IdentityResult> ChangePassword(ChangePasswordRequest request);
        Task SendMailChangeEmail(SendMailChangeEmailRequest request);
        Task<IdentityResult> ChangeEmail(ChangeEmailRequest request);
    }
    public class AccountService : IAccountService
    {
        private readonly IStringLocalizer<AccountService> _stringLocalizer;
        private readonly UserManager<AppUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;

        public AccountService
            (
                IStringLocalizer<AccountService> stringLocalizer,
                UserManager<AppUser> userManager,
                ICurrentUserService currentUserService,
                IConfiguration configuration,
                IMailService mailService
            )
        {
            _stringLocalizer = stringLocalizer;
            _userManager = userManager;
            _currentUserService = currentUserService;
            _configuration = configuration;
            _mailService = mailService;
        }

        public async Task SendMailResetPassword(SendMailResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new Exception(_stringLocalizer["UserIsNotExist"]);
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            string resetUrl = $"{_configuration["ApiUrl"]}/api/accounts/reset-password?code={code}";

            await _mailService.SendAsync(user.Email, "Reset your password", string.Format(_stringLocalizer["ResetPassword"], resetUrl));
        }

        public async Task<IdentityResult> ResetPassword(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new Exception(_stringLocalizer["UserIsNotExist"]);
            }

            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));

            return await _userManager.ResetPasswordAsync(user, code, request.Password);
        }

        public async Task<IdentityResult> ChangePassword(ChangePasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(_currentUserService.UserId);
            if (user == null)
            {
                throw new Exception(_stringLocalizer["UserIsNotExist"]);
            }

            return await _userManager.ChangePasswordAsync(user, request.OldPassWord, request.NewPassWord);
        }

        public async Task SendMailChangeEmail(SendMailChangeEmailRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                throw new Exception(_stringLocalizer["UserIsNotExist"]);
            }
            var code = await _userManager.GenerateChangeEmailTokenAsync(user, request.NewEmail);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            string resetUrl = $"{_configuration["ApiUrl"]}/api/accounts/change-email?userId={request.UserId}&code={code}";

            await _mailService.SendAsync(request.NewEmail, "Change your email", string.Format(_stringLocalizer["ChangeEmail"], resetUrl));
        }

        public async Task<IdentityResult> ChangeEmail(ChangeEmailRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                throw new Exception(_stringLocalizer["UserIsNotExist"]);
            }

            await _userManager.SetUserNameAsync(user, request.NewEmail);
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
            return await _userManager.ChangeEmailAsync(user, request.NewEmail, code);
        }
    }
}
