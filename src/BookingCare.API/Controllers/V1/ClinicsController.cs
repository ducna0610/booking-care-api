using Asp.Versioning;
using BookingCare.API.Controllers.Common.Wrapper;
using BookingCare.Application.DTOs.Requests.Clinic;
using BookingCare.Application.DTOs.Responses;
using BookingCare.Application.DTOs.Responses.Clinic;
using BookingCare.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingCare.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ClinicsController : ControllerBase
{
    private readonly ILogger<ClinicsController> _logger;
    private readonly IClinicService _clinicService;

    public ClinicsController(ILogger<ClinicsController> logger, IClinicService clinicService)
    {
        _logger = logger;
        _clinicService = clinicService;
    }

    /// <summary>
    /// Get pagination clinic
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(SuccessResult<PaginationResponse<ClinicDetailResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ToPagination([FromQuery] PaginationClinicRequest request)
    {
        try
        {
            var response = await _clinicService.ToPagination(request);
            if (response.Items?.Count > 0)
            {
                return Ok(new SuccessResult<PaginationResponse<ClinicDetailResponse>>(response));
            }
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError($"The method {nameof(ClinicService.ToPagination)} caused an exception", ex);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Get list name clinic
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
            var response = await _clinicService.GetListName();
            if (response.Count() > 0)
            {
                return Ok(new SuccessResult<IEnumerable<NameResponse>>(response));
            }
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError($"The method {nameof(ClinicService.GetListName)} caused an exception", ex);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Get clinic by id
    /// </summary>
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(SuccessResult<ClinicDetailResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var response = await _clinicService.GetById(id);
            if (response.Id != Guid.Empty)
            {
                return Ok(new SuccessResult<ClinicDetailResponse>(response));
            }
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError($"The method {nameof(ClinicService.GetById)} caused an exception", ex);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Get clinic by id for admin
    /// </summary>
    [HttpGet]
    [Route("admin/{id}")]
    [ProducesResponseType(typeof(SuccessResult<ClinicDetailForAdminResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByIdForAdmin(Guid id)
    {
        try
        {
            var response = await _clinicService.GetByIdForAdmin(id);
            if (response.Name != null)
            {
                return Ok(new SuccessResult<ClinicDetailForAdminResponse>(response));
            }
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError($"The method {nameof(ClinicService.GetByIdForAdmin)} caused an exception", ex);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Create clinic
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(SuccessResult<ClinicResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromForm] CreateClinicRequest request)
    {
        try
        {
            var response = await _clinicService.Create(request);

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, new SuccessResult<ClinicResponse>(response));
        }
        catch (Exception ex)
        {
            _logger.LogError($"The method {nameof(ClinicService.Create)} caused an exception", ex);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Update clinic
    /// </summary>
    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(typeof(SuccessResult<ClinicResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(Guid id, [FromForm] UpdateClinicRequest request)
    {
        try
        {
            var response = await _clinicService.Update(id, request);
            return Ok(new SuccessResult<ClinicResponse>(response));
        }
        catch (Exception ex)
        {
            _logger.LogError($"The method {nameof(ClinicService.Update)} caused an exception", ex);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Delete clinic
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
            await _clinicService.Delete(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"The method {nameof(ClinicService.Delete)} caused an exception", ex);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }


    /// <summary>
    /// Import clinic
    /// </summary>
    [HttpPost]
    [Route("import")]
    [ProducesResponseType(typeof(SuccessResult<IEnumerable<ClinicResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Import(IFormFile file)
    {
        try
        {
            var response = await _clinicService.Import(file);
            return Ok(new SuccessResult<IEnumerable<ClinicResponse>>(response));
        }
        catch (Exception ex)
        {
            _logger.LogError($"The method {nameof(ClinicService.Create)} caused an exception", ex);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    ///Export clinic
    /// </summary>
    [HttpPost]
    [Route("export")]
    [ProducesResponseType(typeof(SuccessResult<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Export()
    {
        try
        {
            return Ok(new SuccessResult<string>(await _clinicService.Export()));
        }
        catch (Exception ex)
        {
            _logger.LogError($"The method {nameof(ClinicService.Create)} caused an exception", ex);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }
}
