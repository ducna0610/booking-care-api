using Asp.Versioning;
using BookingCare.API.Controllers.Common.Wrapper;
using BookingCare.Application.DTOs.Requests.Payment;
using BookingCare.Application.DTOs.Responses.Payment;
using BookingCare.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingCare.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;
        private readonly IMomoService _momoService;

        public PaymentsController
            (
                IVnPayService vnPayService,
                IMomoService momoService
            )
        {
            _vnPayService = vnPayService;
            _momoService = momoService;
        }

        [HttpPost]
        [Route("vnpay-create")]
        [ProducesResponseType(typeof(SuccessResult<PaymentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VnPayCreate([FromForm] PaymentRequest request)
        {
            try
            {
                return Ok(new SuccessResult<PaymentResponse>(await _vnPayService.CreatePaymentUrl(request, HttpContext)));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult(ex.Message));
            }
        }

        [HttpGet]
        [Route("vnpay-return")]
        [ProducesResponseType(typeof(SuccessResult<PaymentReturnResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VnPayReturn([FromQuery] VnPayRequest request)
        {
            try
            {
                return Ok(new SuccessResult<PaymentReturnResponse>(_vnPayService.PaymentExecuteReturn(request)));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult(ex.Message));
            }
        }

        [HttpGet]
        [Route("vnpay-ipn")]
        [ProducesResponseType(typeof(SuccessResult<VnpayPayIpnResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VnPayIpn([FromQuery] VnPayRequest request)
        {
            try
            {
                return Ok(new SuccessResult<VnpayPayIpnResponse>(await _vnPayService.PaymentExecuteIpn(request)));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult(ex.Message));
            }
        }

        [HttpPost]
        [Route("momo-create")]
        [ProducesResponseType(typeof(SuccessResult<PaymentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> MomoCreate([FromForm] PaymentRequest request)
        {
            try
            {
                return Ok(new SuccessResult<PaymentResponse>(await _momoService.CreatePaymentUrl(request)));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult(ex.Message));
            }
        }

        [HttpGet]
        [Route("momo-return")]
        [ProducesResponseType(typeof(SuccessResult<PaymentReturnResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> MomoReturn([FromQuery] MomoRequest request)
        {
            try
            {
                return Ok(new SuccessResult<PaymentReturnResponse>(_momoService.PaymentExecuteReturn(request)));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult(ex.Message));
            }
        }

        [HttpGet]
        [Route("momo-ipn")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> MomoIpn([FromQuery] MomoRequest request)
        {
            try
            {
                await _momoService.PaymentExecuteIpn(request);
                return Ok(new SuccessResult());
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult(ex.Message));
            }
        }
    }
}
