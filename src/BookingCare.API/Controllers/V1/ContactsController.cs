using Asp.Versioning;
using BookingCare.API.Controllers.Common.Wrapper;
using BookingCare.Application.DTOs.Requests;
using BookingCare.Application.DTOs.Requests.Contact;
using BookingCare.Application.DTOs.Responses;
using BookingCare.Application.DTOs.Responses.Contact;
using BookingCare.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingCare.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly ILogger<ClinicsController> _logger;
        private readonly IContactService _contactService;

        public ContactsController(ILogger<ClinicsController> logger, IContactService contactService)
        {
            _logger = logger;
            _contactService = contactService;
        }

        /// <summary>
        /// Get pagination contact
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(SuccessResult<PaginationResponse<ContactResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ToPagination([FromQuery] PaginationRequest request)
        {
            try
            {
                var response = await _contactService.ToPagination(request);
                if (response.Items?.Count > 0)
                {
                    return Ok(new SuccessResult<PaginationResponse<ContactResponse>>(response));
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"The method {nameof(ContactService.ToPagination)} caused an exception", ex);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Get by id contact
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(SuccessResult<ContactResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var response = await _contactService.GetById(id);
                if (response.Id != Guid.Empty)
                {
                    return Ok(new SuccessResult<ContactResponse>(response));
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"The method {nameof(ContactService.GetById)} caused an exception", ex);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Create contact
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(SuccessResult<ContactResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromForm] CreateContactRequest request)
        {
            try
            {
                var response = await _contactService.Create(request);

                return CreatedAtAction(nameof(GetById), new { id = response.Id }, new SuccessResult<ContactResponse>(response));
            }
            catch (Exception ex)
            {
                _logger.LogError($"The method {nameof(ContactService.Create)} caused an exception", ex);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Delete contact
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
                await _contactService.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"The method {nameof(ContactService.Create)} caused an exception", ex);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
