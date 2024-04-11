using Asp.Versioning;
using BookingCare.API.Controllers.Common.Wrapper;
using BookingCare.Application.DTOs.Responses;
using BookingCare.Application.DTOs.Responses.User;
using BookingCare.Application.Services;
using BookingCare.Application.Utils;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Serilog;

namespace BookingCare.API.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<TestController> _stringLocalizer;
        private readonly IClinicService _clinicService;
        private readonly ILogger<TestController> _logger;

        public TestController
            (
            IStringLocalizer<TestController> stringLocalizer,
            IExcelService excelService,
            IClinicService clinicService,
            ILogger<TestController> logger
            )
        {
            _excelService = excelService;
            _stringLocalizer = stringLocalizer;
            _clinicService = clinicService;
            _logger = logger ?? throw new ArgumentException(nameof(logger));
        }

        [HttpGet]
        [Route("export-excel")]
        [ProducesResponseType(typeof(SuccessResult<UserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Export()
        {
            try
            {
                return Ok(await _excelService.ExportAsync<NameResponse>(await _clinicService.GetListName()));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception Caught");
                return BadRequest(new ErrorResult(ex.Message));
            }
        }

        /// <summary>
        /// This API returns a list of weather forecasts.
        /// </summary>
        /// <remarks>
        /// Possible values could be:
        ///
        ///     "Freezing", "Bracing", "Chilly", "Cool", "Mild",
        ///     "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ///
        /// Just for demonstration
        ///
        ///     GET api/v1/WeatherForecast
        ///     {
        ///     }
        ///     curl -X GET "https://server-url/api/v1/WeatherForecast" -H  "accept: text/plain"
        ///
        /// </remarks>
        [HttpGet]
        [Route("Enqueue")]
        public IActionResult Enqueue()
        {
            _logger.LogInformation("Hello");
            Log.Information("Hi");

            BackgroundJob.Enqueue(() => Console.WriteLine("just a test"));
            return Ok();
        }

        [HttpGet]
        [Route("Schedule")]
        public IActionResult Schedule()
        {
            BackgroundJob.Schedule(() => Console.WriteLine("just a test"), TimeSpan.FromSeconds(10));
            return Ok();
        }

        [HttpGet]
        [Route("AddOrUpdate")]
        public IActionResult AddOrUpdate()
        {
            RecurringJob.AddOrUpdate("myrecurringjob", () => Console.WriteLine("just a test"), Cron.Daily);
            return Ok();
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var message = _stringLocalizer.GetAllStrings();
            return Ok(message);
        }
    }
}
