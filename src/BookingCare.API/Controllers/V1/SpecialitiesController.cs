using Asp.Versioning;
using BookingCare.API.Controllers.Common.Wrapper;
using BookingCare.Application.DTOs.Requests.Speciality;
using BookingCare.Application.DTOs.Responses;
using BookingCare.Application.DTOs.Responses.Speciality;
using BookingCare.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingCare.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class SpecialitiesController : ControllerBase
{
    private readonly ILogger<SpecialitiesController> _logger;
    private readonly ISpecialityService _specialityService;

    public SpecialitiesController(ILogger<SpecialitiesController> logger, ISpecialityService specialityService)
    {
        _logger = logger;
        _specialityService = specialityService;
    }

    /// <summary>
    /// Get pagination speciality
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(SuccessResult<PaginationResponse<SpecialityDetailResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ToPagination([FromQuery] PaginationSpecialityRequest request)
    {
        try
        {
            var response = await _specialityService.ToPagination(request);
            if (response.Items?.Count > 0)
            {
                return Ok(new SuccessResult<PaginationResponse<SpecialityDetailResponse>>(response));
            }
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError($"The method {nameof(SpecialityService.ToPagination)} caused an exception", ex);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Get all speciality
    /// </summary>
    [HttpGet]
    [Route("all")]
    [ProducesResponseType(typeof(SuccessResult<IEnumerable<SpecialityDetailResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var response = await _specialityService.GetAll();
            if (response.Count() > 0)
            {
                return Ok(new SuccessResult<IEnumerable<SpecialityDetailResponse>>(response));
            }
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError($"The method {nameof(SpecialityService.GetAll)} caused an exception", ex);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// List name speciality
    /// </summary>
    [HttpGet]
    [Route("list-name")]
    [ProducesResponseType(typeof(SuccessResult<IEnumerable<NameResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetListName()
    {
        try
        {
            var response = await _specialityService.GetListName();
            if (response.Count() > 0)
            {
                return Ok(new SuccessResult<IEnumerable<NameResponse>>(response));
            }
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError($"The method {nameof(SpecialityService.GetListName)} caused an exception", ex);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Get by id speciality
    /// </summary>
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(SuccessResult<SpecialityResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var response = await _specialityService.GetById(id);
            if (response.Id != Guid.Empty)
            {
                return Ok(new SuccessResult<SpecialityResponse>(response));
            }
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError($"The method {nameof(SpecialityService.GetById)} caused an exception", ex);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Create speciality
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(SuccessResult<SpecialityResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromForm] CreateSpecialityRequest request)
    {
        try
        {
            var response = await _specialityService.Create(request);

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, new SuccessResult<SpecialityResponse>(response));
        }
        catch (Exception ex)
        {
            _logger.LogError($"The method {nameof(SpecialityService.Create)} caused an exception", ex);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Update speciality
    /// </summary>
    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(typeof(SuccessResult<SpecialityResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(Guid id, [FromForm] UpdateSpecialityRequest request)
    {
        try
        {
            var response = await _specialityService.Update(id, request);
            return Ok(new SuccessResult<SpecialityResponse>(response));
        }
        catch (Exception ex)
        {
            _logger.LogError($"The method {nameof(SpecialityService.Create)} caused an exception", ex);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Delete speciality
    /// </summary>
    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _specialityService.Delete(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"The method {nameof(SpecialityService.Delete)} caused an exception", ex);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Import speciality
    /// </summary>
    [HttpPost]
    [Route("import")]
    [ProducesResponseType(typeof(SuccessResult<IEnumerable<SpecialityResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Import(IFormFile file)
    {
        try
        {
            return Ok(new SuccessResult<IEnumerable<SpecialityResponse>>(await _specialityService.Import(file)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResult(ex.Message));
        }
    }
}