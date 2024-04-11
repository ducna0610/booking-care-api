using AutoFixture;
using BookingCare.API.Controllers.Common.Wrapper;
using BookingCare.API.Controllers.V1;
using BookingCare.Application.DTOs.Requests.Booking;
using BookingCare.Application.DTOs.Responses;
using BookingCare.Application.DTOs.Responses.Booking;
using BookingCare.Application.Services;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookingCare.Tests.Controllers;

public class BookingsControllerTests
{
    private readonly Fixture _fixture;
    private readonly ILogger<BookingsController> _logger;
    private readonly IBookingService _bookingService;
    private readonly BookingsController _sut;

    public BookingsControllerTests()
    {
        _fixture = new Fixture();
        _logger = A.Fake<ILogger<BookingsController>>();
        _bookingService = A.Fake<IBookingService>();
        _sut = new BookingsController(_logger, _bookingService);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task ToPagination_WhenThereAreBookings_ShouldReturnPageOfBookingDetailsWithStatusCode200OK()
    {
        // Arrange
        var request = _fixture.Create<PaginationBookingRequest>();
        var response = _fixture.Create<PaginationResponse<BookingDetailResponse>>();
        A.CallTo(() => _bookingService.ToPagination(A<PaginationBookingRequest>._)).Returns(response);

        // Act
        var actual = await _sut.ToPagination(request);

        // Assert
        A.CallTo(() => _bookingService.ToPagination(A<PaginationBookingRequest>._)).MustHaveHappenedOnceExactly();
        var actionResult = Assert.IsType<OkObjectResult>(actual);
        Assert.IsType<SuccessResult<PaginationResponse<BookingDetailResponse>>>(actionResult.Value);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task ToPagination_WhenThereAreNoBookingsFound_ShouldReturnStatusCode404NotFound()
    {
        // Arrange
        var request = _fixture.Create<PaginationBookingRequest>();
        var response = new PaginationResponse<BookingDetailResponse>();
        A.CallTo(() => _bookingService.ToPagination(A<PaginationBookingRequest>._)).Returns(response);

        // Act
        var actual = await _sut.ToPagination(request) as StatusCodeResult;

        // Assert
        A.CallTo(() => _bookingService.ToPagination(A<PaginationBookingRequest>._)).MustHaveHappenedOnceExactly();
        Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task ToPagination_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
    {
        // Arrange
        var request = _fixture.Create<PaginationBookingRequest>();
        A.CallTo(() => _bookingService.ToPagination(A<PaginationBookingRequest>._)).Throws<Exception>();

        // Act
        var actual = await _sut.ToPagination(request) as StatusCodeResult;

        // Assert
        A.CallTo(() => _bookingService.ToPagination(A<PaginationBookingRequest>._)).MustHaveHappenedOnceExactly();
        A.CallTo(_logger).Where(
                call => call.Method.Name == "Log"
                && call.GetArgument<LogLevel>(0) == LogLevel.Error)
            .MustHaveHappened(1, Times.Exactly);
        Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetById_WhenThereIsBooking_ShouldReturnBookingWithStatusCode200OK()
    {
        // Arrange
        var request = _fixture.Create<Guid>();
        var response = _fixture.Create<BookingDetailResponse>();
        A.CallTo(() => _bookingService.GetById(A<Guid>._)).Returns(response);

        // Act
        var actual = await _sut.GetById(request);

        // Assert
        A.CallTo(() => _bookingService.GetById(A<Guid>._)).MustHaveHappenedOnceExactly();
        var actionResult = Assert.IsType<OkObjectResult>(actual);
        Assert.IsType<SuccessResult<BookingDetailResponse>>(actionResult.Value);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetById_WhenThereAreNoClinicFound_ShouldReturnStatusCode404NotFound()
    {
        // Arrange
        var request = _fixture.Create<Guid>();
        var response = new BookingDetailResponse();
        A.CallTo(() => _bookingService.GetById(A<Guid>._)).Returns(response);

        // Act
        var actual = await _sut.GetById(request) as StatusCodeResult;

        // Assert
        A.CallTo(() => _bookingService.GetById(A<Guid>._)).MustHaveHappenedOnceExactly();
        Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetById_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        A.CallTo(() => _bookingService.GetById(A<Guid>._)).Throws<Exception>();

        // Act
        var actual = await _sut.GetById(id) as StatusCodeResult;

        // Assert
        A.CallTo(() => _bookingService.GetById(A<Guid>._)).MustHaveHappenedOnceExactly();
        A.CallTo(_logger).Where(
                call => call.Method.Name == "Log"
                && call.GetArgument<LogLevel>(0) == LogLevel.Error)
            .MustHaveHappened(1, Times.Exactly);
        Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
    }
    [Fact]
    [Trait("HttpVerb", "POST")]
    public async Task Create_WhenValid_ShouldReturnStatusCode201Created()
    {
        // Arrange
        _fixture.Register<IFormFile>(() => null);
        var request = _fixture.Create<CreateBookingRequest>();
        var response = _fixture.Create<BookingResponse>();
        A.CallTo(() => _bookingService.Create(A<CreateBookingRequest>._)).Returns(response);

        // Act
        var actual = await _sut.Create(request);

        // Assert
        A.CallTo(() => _bookingService.Create(A<CreateBookingRequest>._)).MustHaveHappenedOnceExactly();
        var actionResult = Assert.IsType<CreatedAtActionResult>(actual);
        Assert.IsType<SuccessResult<BookingResponse>>(actionResult.Value);
    }

    [Fact]
    [Trait("HttpVerb", "POST")]
    public async Task Create_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
    {
        // Arrange
        var req = _fixture.Create<CreateBookingRequest>();
        A.CallTo(() => _bookingService.Create(A<CreateBookingRequest>._)).Throws<Exception>();

        // Act
        var actual = await _sut.Create(req) as StatusCodeResult;

        // Assert
        A.CallTo(() => _bookingService.Create(A<CreateBookingRequest>._)).MustHaveHappenedOnceExactly();
        A.CallTo(_logger).Where(
                call => call.Method.Name == "Log"
                && call.GetArgument<LogLevel>(0) == LogLevel.Error)
            .MustHaveHappened(1, Times.Exactly);
        Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
    }

    [Fact]
    [Trait("HttpVerb", "PUT")]
    public async Task Update_WhenValid_ShouldReturnClinicStatusCode200OK()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var request = _fixture.Create<UpdateStatusBookingRequest>();
        var response = _fixture.Create<BookingResponse>();
        A.CallTo(() => _bookingService.UpdateStatus(A<Guid>._, A<UpdateStatusBookingRequest>._)).Returns(response);

        // Act
        var actual = await _sut.Confirm(id, request);

        // Assert
        A.CallTo(() => _bookingService.UpdateStatus(A<Guid>._, A<UpdateStatusBookingRequest>._)).MustHaveHappenedOnceExactly();
        var actionResult = Assert.IsType<OkObjectResult>(actual);
        Assert.IsType<SuccessResult<BookingResponse>>(actionResult.Value);
    }

    [Fact]
    [Trait("HttpVerb", "PUT")]
    public async Task Update_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var request = _fixture.Create<UpdateStatusBookingRequest>();
        A.CallTo(() => _bookingService.UpdateStatus(A<Guid>._, A<UpdateStatusBookingRequest>._)).Throws<Exception>();

        // Act
        var actual = await _sut.Confirm(id, request) as StatusCodeResult;

        // Assert
        A.CallTo(() => _bookingService.UpdateStatus(A<Guid>._, A<UpdateStatusBookingRequest>._)).MustHaveHappenedOnceExactly();
        A.CallTo(_logger).Where(
                call => call.Method.Name == "Log"
                && call.GetArgument<LogLevel>(0) == LogLevel.Error)
            .MustHaveHappened(1, Times.Exactly);
        Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
    }
}
