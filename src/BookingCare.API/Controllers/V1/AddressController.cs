using Asp.Versioning;
using BookingCare.API.Controllers.Common.Wrapper;
using BookingCare.Application.DTOs.Responses.Address;
using BookingCare.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingCare.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AddressController : ControllerBase
{
    private readonly ILogger<AddressController> _logger;
    private readonly IAddressService _addressService;

    public AddressController(ILogger<AddressController> logger, IAddressService addressService)
    {
        _logger = logger;
        _addressService = addressService;
    }

    /// <summary>
    /// Get provinces
    /// </summary>
    [HttpGet]
    [Route("provinces")]
    [ProducesResponseType(typeof(SuccessResult<IEnumerable<ProvinceResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProvinces()
    {
        try
        {
            var response = await _addressService.GetProvinces();
            if (response.Count() > 0)
            {
                return Ok(new SuccessResult<IEnumerable<ProvinceResponse>>(response));
            }

            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError($"The method {nameof(AddressService.GetProvinces)} caused an exception", ex);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Get districts
    /// </summary>
    [HttpGet]
    [Route("districts/{provinceId}")]
    [ProducesResponseType(typeof(SuccessResult<IEnumerable<DistrictResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDistrictsByProvinceId(int provinceId)
    {
        try
        {
            var response = await _addressService.GetDistrictsByProvinceId(provinceId);
            if (response.Count() > 0)
            {
                return Ok(new SuccessResult<IEnumerable<DistrictResponse>>(response));
            }

            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError($"The method {nameof(AddressService.GetDistrictsByProvinceId)} caused an exception", ex);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Get wards
    /// </summary>
    [HttpGet]
    [Route("wards/{districtId}")]
    [ProducesResponseType(typeof(SuccessResult<IEnumerable<WardResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetWardsByDistrictId(int districtId)
    {
        try
        {
            var response = await _addressService.GetWardsByDistrictId(districtId);
            if (response.Count() > 0)
            {
                return Ok(new SuccessResult<IEnumerable<WardResponse>>(response));
            }

            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError($"The method {nameof(AddressService.GetWardsByDistrictId)} caused an exception", ex);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }
}
