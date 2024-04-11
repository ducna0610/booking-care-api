using Asp.Versioning;
using BookingCare.API.Controllers.Common.Wrapper;
using BookingCare.Application.DTOs.Requests.User;
using BookingCare.Application.DTOs.Responses;
using BookingCare.Application.DTOs.Responses.User;
using BookingCare.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingCare.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IAppUserService _appUserService;

        public UsersController(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(SuccessResult<PaginationResponse<UserDetailResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ToPagination([FromQuery] PaginationUserRequest request)
            => Ok(new SuccessResult<PaginationResponse<UserDetailResponse>>(await _appUserService.ToPagination(request)));

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(SuccessResult<UserDetailResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(string id)
            => Ok(new SuccessResult<UserDetailResponse>(await _appUserService.GetById(id)));

        [HttpPost]
        [ProducesResponseType(typeof(SuccessResult<UserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromForm] CreateUserRequest request)
        {
            try
            {
                return Ok(new SuccessResult<UserResponse>(await _appUserService.Create(request)));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult(ex.Message));
            }
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(SuccessResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, [FromForm] UpdateUserRequest request)
        {
            try
            {
                var result = await _appUserService.Update(id.ToString(), request);
                return Ok(new SuccessResult<bool>(result.Succeeded));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult(ex.Message));
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _appUserService.Delete(id.ToString());
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult(ex.Message));
            }
        }
    }
}
