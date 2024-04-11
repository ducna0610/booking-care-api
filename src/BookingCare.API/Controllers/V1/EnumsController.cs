using Asp.Versioning;
using BookingCare.API.Controllers.Common.Wrapper;
using BookingCare.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingCare.API.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class EnumsController : ControllerBase
{
    private readonly ILogger<EnumsController> _logger;
    private readonly IEnumService _enumService;
    public EnumsController(ILogger<EnumsController> logger, IEnumService enumService)
    {
        _logger = logger;
        _enumService = enumService;
    }

    /// <summary>
    /// Get gender
    /// </summary>
    [HttpGet]
    [Route("gender")]
    [ProducesResponseType(typeof(SuccessResult<IEnumerable<KeyValuePair<int, string>>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetGenderEnum()
    {
        try
        {
            var response = _enumService.GetGenderEnum();
            if (response.Count() > 0)
            {
                return Ok(new SuccessResult<IEnumerable<KeyValuePair<int, string>>>(response));
            }
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError($"The method {nameof(EnumService.GetGenderEnum)} caused an exception", ex);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Get time select
    /// </summary>
    [HttpGet]
    [Route("time-select")]
    [ProducesResponseType(typeof(SuccessResult<IEnumerable<KeyValuePair<int, string>>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTimeSelectEnum()
    {
        try
        {
            var response = _enumService.GetTimeSelectEnum();
            if (response.Count() > 0)
            {
                return Ok(new SuccessResult<IEnumerable<KeyValuePair<int, string>>>(response));
            }
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError($"The method {nameof(EnumService.GetTimeSelectEnum)} caused an exception", ex);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Get status
    /// </summary>
    [HttpGet]
    [Route("status")]
    [ProducesResponseType(typeof(SuccessResult<IEnumerable<KeyValuePair<int, string>>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStatusEnum()
    {
        try
        {
            var response = _enumService.GetStatusEnum();
            if (response.Count() > 0)
            {
                return Ok(new SuccessResult<IEnumerable<KeyValuePair<int, string>>>(response));
            }
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError($"The method {nameof(EnumService.GetStatusEnum)} caused an exception", ex);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }
}
