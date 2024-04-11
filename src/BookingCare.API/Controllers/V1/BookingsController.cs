using Asp.Versioning;
using BookingCare.API.Controllers.Common.Wrapper;
using BookingCare.Application.DTOs.Requests.Booking;
using BookingCare.Application.DTOs.Responses;
using BookingCare.Application.DTOs.Responses.Booking;
using BookingCare.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingCare.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly ILogger<BookingsController> _logger;
        private readonly IBookingService _bookingService;
        public BookingsController(ILogger<BookingsController> logger, IBookingService bookingService)
        {
            _logger = logger;
            _bookingService = bookingService;
        }

        /// <summary>
        /// Get pagination booking
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(SuccessResult<PaginationResponse<BookingDetailResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ToPagination([FromQuery] PaginationBookingRequest request)
        {
            try
            {
                var response = await _bookingService.ToPagination(request);
                if (response.Items?.Count > 0)
                {
                    return Ok(new SuccessResult<PaginationResponse<BookingDetailResponse>>(response));
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"The method {nameof(BookingService.ToPagination)} caused an exception", ex);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Get booking by id
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(SuccessResult<BookingDetailResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var response = await _bookingService.GetById(id);
                if (response.Id != Guid.Empty)
                {
                    return Ok(new SuccessResult<BookingDetailResponse>(response));
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"The method {nameof(BookingService.GetById)} caused an exception", ex);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Create booking
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(SuccessResult<BookingResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromForm] CreateBookingRequest request)
        {
            try
            {
                var response = await _bookingService.Create(request);

                return CreatedAtAction(nameof(GetById), new { id = response.Id }, new SuccessResult<BookingResponse>(response));
            }
            catch (Exception ex)
            {
                _logger.LogError($"The method {nameof(BookingService.Create)} caused an exception", ex);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Update booking
        /// </summary>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(SuccessResult<BookingResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Confirm(Guid id, [FromForm] UpdateStatusBookingRequest request)
        {
            try
            {
                var response = await _bookingService.UpdateStatus(id, request);
                return Ok(new SuccessResult<BookingResponse>(response));
            }
            catch (Exception ex)
            {
                _logger.LogError($"The method {nameof(BookingService.UpdateStatus)} caused an exception", ex);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
