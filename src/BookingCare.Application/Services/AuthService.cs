using AutoMapper;
using BookingCare.Application.DTOs.Requests.Auth;
using BookingCare.Application.DTOs.Responses.Auth;
using BookingCare.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Localization;
using Hangfire;
using BookingCare.Application.Utils;

namespace BookingCare.Application.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> SignUp(SignUpRequest request);
        Task<TokenResponse> SignIn(SignInRequest request);
        Task<TokenResponse> RefreshToken(TokenRequest request);
        Task<IdentityResult> ConfirmEmail(ConfirmEmailRequest request);
    }

    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AuthService> _stringLocalizer;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;

        public AuthService
            (
                IMapper mapper,
                IStringLocalizer<AuthService> stringLocalizer,
                UserManager<AppUser> userManager,
                SignInManager<AppUser> signInManager,
                IConfiguration configuration,
                IMailService mailService
            )
        {
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _mailService = mailService;
        }

        public async Task<AuthResponse> SignUp(SignUpRequest request)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var user = await _userManager.FindByEmailAsync(request.Email);

                    if (user != null)
                    {
                        throw new Exception(_stringLocalizer["UserIsExist"]);
                    }

                    user = _mapper.Map<AppUser>(request);

                    await _userManager.CreateAsync(user, request.Password);

                    await _userManager.AddToRolesAsync(user, request.Roles);

                    // Get the roles for the user
                    var data = _mapper.Map<AuthResponse>(user);

                    data.Roles = await _userManager.GetRolesAsync(user);


                    var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var validEmailToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmEmailToken));
                    string confirmUrl = $"{_configuration["ApiUrl"]}/api/auth/confirm-email?userid={user.Id}&token={validEmailToken}";
                    BackgroundJob.Enqueue(() => _mailService.SendAsync(user.Email, "Confirm email", string.Format(_stringLocalizer["ConfirmEmail"], confirmUrl)));
                    
                    scope.Complete();

                    return data;
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;
                }
            }
        }

        public async Task<IdentityResult> ConfirmEmail(ConfirmEmailRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                throw new Exception(_stringLocalizer["UserIsNotExist"]);
            }

            var decodedToken = WebEncoders.Base64UrlDecode(request.Token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);
            var result = await _userManager.ConfirmEmailAsync(user, normalToken);

            return result;
        }

        public async Task<TokenResponse> SignIn(SignInRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new Exception(_stringLocalizer["ErrorSignIn"]);
            }

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);

            if (!result.Succeeded)
            {
                throw new Exception(_stringLocalizer["ErrorSignIn"]);
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GenerateToken(authClaims);
            var refreshToken = GenerateRefreshToken();

            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

            await _userManager.UpdateAsync(user);

            return (new TokenResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken
            });
        }

        public async Task<TokenResponse> RefreshToken(TokenRequest request)
        {
            string? accessToken = request.AccessToken;
            string? refreshToken = request.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);

            if (principal == null)
            {
                throw new Exception(_stringLocalizer["TokenInValid"]);
            }

            var userPrincipal = GetPrincipalFromExpiredToken(accessToken);
            var userEmail = userPrincipal.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(userEmail);

            //Check refreshtoken exist in DB
            if (user.RefreshToken != refreshToken)
            {
                throw new Exception(_stringLocalizer["RefreshTokenInValid"]);
            }

            //Check accessToken expire?
            var utcExpireDateAccessToken = long.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expireDateAccessToken = ConvertUnixTimeToDateTime(utcExpireDateAccessToken);

            if (expireDateAccessToken > DateTime.UtcNow)
            {
                throw new Exception(_stringLocalizer["TokenNotExpire"]);
            }

            var newAccessToken = GenerateToken(principal.Claims.ToList());
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return new TokenResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken
            };
        }

        private JwtSecurityToken GenerateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityDays"], out int tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException(_stringLocalizer["TokenInValid"]);

            return principal;
        }

        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();

            return dateTimeInterval;
        }
    }
}