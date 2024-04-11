using Asp.Versioning;
using BookingCare.API.Controllers.Common.Wrapper;
using BookingCare.Application.DTOs.Requests.Doctor;
using BookingCare.Application.DTOs.Responses;
using BookingCare.Application.DTOs.Responses.Doctor;
using BookingCare.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingCare.API.Controllers.V1
{
    [ApiController]
    //[AuthorizeRoles(RoleName.ADMIN, RoleName.DOCTOR)]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly ILogger<DoctorsController> _logger;
        private readonly IDoctorService _doctorService;

        public DoctorsController(ILogger<DoctorsController> logger, IDoctorService doctorService)
        {
            _logger = logger;
            _doctorService = doctorService;
        }

        /// <summary>
        /// Get pagination doctor
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(SuccessResult<PaginationResponse<DoctorInfoDetailResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ToPagination([FromQuery] PaginationDoctorRequest request)
        {
            try
            {
                var response = await _doctorService.ToPagination(request);
                if (response.Items?.Count > 0)
                {
                    return Ok(new SuccessResult<PaginationResponse<DoctorInfoDetailResponse>>(response));
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"The method {nameof(DoctorService.ToPagination)} caused an exception", ex);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Get doctor by id
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(SuccessResult<DoctorInfoDetailResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var response = await _doctorService.GetById(id);
                if (response.Id != Guid.Empty)
                {
                    return Ok(new SuccessResult<DoctorInfoDetailResponse>(response));
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"The method {nameof(DoctorService.GetById)} caused an exception", ex);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Create doctor
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(SuccessResult<DoctorInfoResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromForm] CreateDoctorRequest request)
        {
            try
            {
                var response = await _doctorService.Create(request);

                return CreatedAtAction(nameof(GetById), new { id = response.Id }, new SuccessResult<DoctorInfoResponse>(response));
            }
            catch (Exception ex)
            {
                _logger.LogError($"The method {nameof(DoctorService.Create)} caused an exception", ex);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Update doctor
        /// </summary>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(SuccessResult<DoctorInfoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Guid id, [FromForm] UpdateDoctorRequest request)
        {
            try
            {
                var response = await _doctorService.Update(id, request);
                return Ok(new SuccessResult<DoctorInfoResponse>(response));
            }
            catch (Exception ex)
            {
                _logger.LogError($"The method {nameof(DoctorService.Update)} caused an exception", ex);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Get schedule
        /// </summary>
        [HttpGet]
        [Route("get-schedule")]
        [ProducesResponseType(typeof(SuccessResult<ScheduleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSchedule([FromQuery] GetScheduleRequest request)
        {
            try
            {
                var response = await _doctorService.GetSchedule(request);
                if (response.Count > 0)
                {
                    return Ok(new SuccessResult<IEnumerable<ScheduleResponse>>(response));
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"The method {nameof(DoctorService.GetSchedule)} caused an exception", ex);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Set schedule
        /// </summary>
        [HttpPut]
        [Route("set-schedule")]
        [ProducesResponseType(typeof(SuccessResult<IEnumerable<ScheduleResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetSchedule([FromForm] SetScheduleRequest request)
        {
            try
            {
                var response = await _doctorService.SetSchedule(request);
                return Ok(new SuccessResult<IEnumerable<ScheduleResponse>>(response));
            }
            catch (Exception ex)
            {
                _logger.LogError($"The method {nameof(DoctorService.SetSchedule)} caused an exception", ex);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
