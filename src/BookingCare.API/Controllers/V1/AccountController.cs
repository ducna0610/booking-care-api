using Asp.Versioning;
using BookingCare.API.Controllers.Common.Wrapper;
using BookingCare.Application.DTOs.Requests.Account;
using BookingCare.Application.DTOs.Requests.User;
using BookingCare.Application.DTOs.Responses.User;
using BookingCare.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingCare.API.Controllers.V1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IAppUserService _appUserService;
        private readonly ICurrentUserService _currentUserService;

        public AccountController
            (
                IAccountService accountService,
                IAppUserService appUserService,
                ICurrentUserService currentUserService
            )
        {
            _accountService = accountService;
            _appUserService = appUserService;
            _currentUserService = currentUserService;
        }


        /// <summary>
        /// Get by id account
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(SuccessResult<UserDetailResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById()
            => Ok(new SuccessResult<UserDetailResponse>(await _appUserService.GetById(_currentUserService.UserId)));

        [HttpPut]
        [ProducesResponseType(typeof(SuccessResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromForm] UpdateUserRequest request)
        {
            try
            {
                var result = await _appUserService.Update(_currentUserService.UserId, request);
                return Ok(new SuccessResult<bool>(result.Succeeded));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult(ex.Message));
            }
        }

        [HttpDelete]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete()
        {
            try
            {
                await _appUserService.Delete(_currentUserService.UserId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult(ex.Message));
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("send-mail-reset-password")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendMailResetPassword([FromQuery] SendMailResetPasswordRequest request)
        {
            try
            {
                await _accountService.SendMailResetPassword(request);
                return Ok(new SuccessResult("Send mail success"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult(ex.Message));
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("reset-password")]
        [ProducesResponseType(typeof(SuccessResult<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword(string code)
        {
            try
            {
                return Ok(new SuccessResult<string>(code));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult(ex.Message));
            }
        }

        [AllowAnonymous]
        [HttpPut]
        [Route("reset-password")]
        [ProducesResponseType(typeof(SuccessResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordRequest request)
        {
            try
            {
                var result = await _accountService.ResetPassword(request);
                return Ok(new SuccessResult<bool>(result.Succeeded));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult(ex.Message));
            }
        }

        [HttpPut]
        [Route("change-password")]
        [ProducesResponseType(typeof(SuccessResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordRequest request)
        {
            try
            {
                var result = await _accountService.ChangePassword(request);
                return Ok(new SuccessResult<bool>(result.Succeeded));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult(ex.Message));
            }
        }


        [HttpGet]
        [Route("send-mail-change-email")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendMailChangeEmail([FromQuery] SendMailChangeEmailRequest request)
        {
            try
            {
                await _accountService.SendMailChangeEmail(request);
                return Ok(new SuccessResult("Send mail success"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult(ex.Message));
            }
        }


        [HttpGet]
        [Route("change-email")]
        [ProducesResponseType(typeof(SuccessResult<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangeEmail(string code)
        {
            try
            {
                return Ok(new SuccessResult<string>(code));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult(ex.Message));
            }
        }

        [HttpPut]
        [Route("change-email")]
        [ProducesResponseType(typeof(SuccessResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangeEmail([FromForm] ChangeEmailRequest request)
        {
            try
            {
                var result = await _accountService.ChangeEmail(request);
                return Ok(new SuccessResult<bool>(result.Succeeded));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult(ex.Message));
            }
        }
    }
}
