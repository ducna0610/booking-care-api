using AutoFixture;
using BookingCare.API.Controllers.Common.Wrapper;
using BookingCare.API.Controllers.v1;
using BookingCare.Application.Services;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookingCare.Tests.Controllers;

public class EnumsControllerTests
{
    private readonly Fixture _fixture;
    private readonly ILogger<EnumsController> _logger;
    private readonly IEnumService _enumService;
    private readonly EnumsController _sut;

    public EnumsControllerTests()
    {
        _fixture = new Fixture();
        _logger = A.Fake<ILogger<EnumsController>>();
        _enumService = A.Fake<IEnumService>();
        _sut = new EnumsController(_logger, _enumService);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetGenderEnum_WhenThereAreGenders_ShouldReturnGendersWithStatusCode200OK()
    {
        // Arrange
        var response = _fixture.Create<IEnumerable<KeyValuePair<int, string>>>();
        A.CallTo(() => _enumService.GetGenderEnum()).Returns(response);

        // Act
        var actual = await _sut.GetGenderEnum();

        // Assert
        A.CallTo(() => _enumService.GetGenderEnum()).MustHaveHappenedOnceExactly();
        var actionResult = Assert.IsType<OkObjectResult>(actual);
        Assert.IsType<SuccessResult<IEnumerable<KeyValuePair<int, string>>>>(actionResult.Value);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetGenderEnum_WhenThereAreNoClinicsFound_ShouldReturnStatusCode404NotFound()
    {
        // Arrange
        var response = new List<KeyValuePair<int, string>>();
        A.CallTo(() => _enumService.GetGenderEnum()).Returns(response);

        // Act
        var actual = await _sut.GetGenderEnum() as StatusCodeResult;

        // Assert
        A.CallTo(() => _enumService.GetGenderEnum()).MustHaveHappenedOnceExactly();
        Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetGenderEnum_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
    {
        // Arrange
        A.CallTo(() => _enumService.GetGenderEnum()).Throws<Exception>();

        // Act
        var actual = await _sut.GetGenderEnum() as StatusCodeResult;

        // Assert
        A.CallTo(() => _enumService.GetGenderEnum()).MustHaveHappenedOnceExactly();
        A.CallTo(_logger).Where(
                call => call.Method.Name == "Log"
                && call.GetArgument<LogLevel>(0) == LogLevel.Error)
            .MustHaveHappened(1, Times.Exactly);
        Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetTimeSelectEnum_WhenThereAreTimeSelects_ShouldReturnTimeSelectsWithStatusCode200OK()
    {
        // Arrange
        var response = _fixture.Create<IEnumerable<KeyValuePair<int, string>>>();
        A.CallTo(() => _enumService.GetTimeSelectEnum()).Returns(response);

        // Act
        var actual = await _sut.GetTimeSelectEnum();

        // Assert
        A.CallTo(() => _enumService.GetTimeSelectEnum()).MustHaveHappenedOnceExactly();
        var actionResult = Assert.IsType<OkObjectResult>(actual);
        Assert.IsType<SuccessResult<IEnumerable<KeyValuePair<int, string>>>>(actionResult.Value);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetTimeSelectEnum_WhenThereAreNoClinicsFound_ShouldReturnStatusCode404NotFound()
    {
        // Arrange
        var response = new List<KeyValuePair<int, string>>();
        A.CallTo(() => _enumService.GetTimeSelectEnum()).Returns(response);

        // Act
        var actual = await _sut.GetTimeSelectEnum() as StatusCodeResult;

        // Assert
        A.CallTo(() => _enumService.GetTimeSelectEnum()).MustHaveHappenedOnceExactly();
        Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetTimeSelectEnum_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
    {
        // Arrange
        A.CallTo(() => _enumService.GetTimeSelectEnum()).Throws<Exception>();

        // Act
        var actual = await _sut.GetTimeSelectEnum() as StatusCodeResult;

        // Assert
        A.CallTo(() => _enumService.GetTimeSelectEnum()).MustHaveHappenedOnceExactly();
        A.CallTo(_logger).Where(
                call => call.Method.Name == "Log"
                && call.GetArgument<LogLevel>(0) == LogLevel.Error)
            .MustHaveHappened(1, Times.Exactly);
        Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
    }


    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetStatusEnum_WhenThereAreTimeSelects_ShouldReturnTimeSelectsWithStatusCode200OK()
    {
        // Arrange
        var response = _fixture.Create<IEnumerable<KeyValuePair<int, string>>>();
        A.CallTo(() => _enumService.GetStatusEnum()).Returns(response);

        // Act
        var actual = await _sut.GetStatusEnum();

        // Assert
        A.CallTo(() => _enumService.GetStatusEnum()).MustHaveHappenedOnceExactly();
        var actionResult = Assert.IsType<OkObjectResult>(actual);
        Assert.IsType<SuccessResult<IEnumerable<KeyValuePair<int, string>>>>(actionResult.Value);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetStatusEnum_WhenThereAreNoClinicsFound_ShouldReturnStatusCode404NotFound()
    {
        // Arrange
        var response = new List<KeyValuePair<int, string>>();
        A.CallTo(() => _enumService.GetStatusEnum()).Returns(response);

        // Act
        var actual = await _sut.GetStatusEnum() as StatusCodeResult;

        // Assert
        A.CallTo(() => _enumService.GetStatusEnum()).MustHaveHappenedOnceExactly();
        Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetStatusEnum_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
    {
        // Arrange
        A.CallTo(() => _enumService.GetStatusEnum()).Throws<Exception>();

        // Act
        var actual = await _sut.GetStatusEnum() as StatusCodeResult;

        // Assert
        A.CallTo(() => _enumService.GetStatusEnum()).MustHaveHappenedOnceExactly();
        A.CallTo(_logger).Where(
                call => call.Method.Name == "Log"
                && call.GetArgument<LogLevel>(0) == LogLevel.Error)
            .MustHaveHappened(1, Times.Exactly);
        Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
    }
}
