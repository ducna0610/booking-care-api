using Asp.Versioning;
using BookingCare.API.Controllers.Common.Wrapper;
using BookingCare.Application.DTOs.Requests.Auth;
using BookingCare.Application.DTOs.Responses.Auth;
using BookingCare.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace BookingCare.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IStringLocalizer<AuthController> _stringLocalizer;

    public AuthController
        (
            IAuthService authService,
            IStringLocalizer<AuthController> stringLocalizer
        )
    {
        _authService = authService;
        _stringLocalizer = stringLocalizer;
    }

    [HttpPost]
    [Route("sign-up")]
    [ProducesResponseType(typeof(SuccessResult<AuthResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignUp([FromForm] SignUpRequest request)
    {
        try
        {
            return Ok(new SuccessResult<AuthResponse>(await _authService.SignUp(request)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResult(ex.Message));
        }
    }

    [HttpGet]
    [Route("confirm-email")]
    [ProducesResponseType(typeof(SuccessResult<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailRequest request)
    {
        try
        {
            return Ok(new SuccessResult<bool>((await _authService.ConfirmEmail(request)).Succeeded));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResult(ex.Message));
        }
    }

    [HttpPost]
    [Route("sign-in")]
    [ProducesResponseType(typeof(SuccessResult<TokenResponse>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignIn([FromForm] SignInRequest request)
    {
        try
        {
            return Ok(new SuccessResult<TokenResponse>(await _authService.SignIn(request), _stringLocalizer["SuccessSignIn"]));
        }
        catch (Exception ex)
        {
            return Unauthorized(new ErrorResult(ex.Message));
        }
    }

    [HttpPost]
    [Route("refresh-token")]
    [ProducesResponseType(typeof(SuccessResult<TokenResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshToken([FromForm] TokenRequest request)
    {
        try
        {
            return Ok(new SuccessResult<TokenResponse>(await _authService.RefreshToken(request)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResult(ex.Message));
        }
    }
}