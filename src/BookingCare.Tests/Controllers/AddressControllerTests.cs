using AutoFixture;
using BookingCare.API.Controllers.Common.Wrapper;
using BookingCare.API.Controllers.V1;
using BookingCare.Application.DTOs.Responses.Address;
using BookingCare.Application.Services;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookingCare.Tests.Controllers;

public class AddressControllerTests
{
    private readonly Fixture _fixture;
    private readonly ILogger<AddressController> _logger;
    private readonly IAddressService _addressService;
    private readonly AddressController _sut;

    public AddressControllerTests()
    {
        _fixture = new Fixture();
        _logger = A.Fake<ILogger<AddressController>>();
        _addressService = A.Fake<IAddressService>();
        _sut = new AddressController(_logger, _addressService);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetProvinces_WhenThereAreProvinces_ShouldReturnProvincesWithStatusCode200OK()
    {
        // Arrange
        var response = _fixture.Create<IEnumerable<ProvinceResponse>>();
        A.CallTo(() => _addressService.GetProvinces()).Returns(response);

        // Act
        var actual = await _sut.GetProvinces();

        // Assert
        A.CallTo(() => _addressService.GetProvinces()).MustHaveHappenedOnceExactly();
        var actionResult = Assert.IsType<OkObjectResult>(actual);
        Assert.IsType<SuccessResult<IEnumerable<ProvinceResponse>>>(actionResult.Value);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetProvinces_WhenThereAreNoProvincesFound_ShouldReturnStatusCode404NotFound()
    {
        // Arrange
        var response = new List<ProvinceResponse>();
        A.CallTo(() => _addressService.GetProvinces()).Returns(response);

        // Act
        var actual = await _sut.GetProvinces() as StatusCodeResult;

        // Assert
        A.CallTo(() => _addressService.GetProvinces()).MustHaveHappenedOnceExactly();
        Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetProvinces_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
    {
        // Arrange
        A.CallTo(() => _addressService.GetProvinces()).Throws<Exception>();

        // Act
        var actual = await _sut.GetProvinces() as StatusCodeResult;

        // Assert
        A.CallTo(() => _addressService.GetProvinces()).MustHaveHappenedOnceExactly();
        A.CallTo(_logger).Where(
                call => call.Method.Name == "Log"
                && call.GetArgument<LogLevel>(0) == LogLevel.Error)
            .MustHaveHappened(1, Times.Exactly);
        Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetDistrictsByProvinceId_WhenThereAreDistricts_ShouldReturnDistrictsWithStatusCode200OK()
    {
        // Arrange
        var id = _fixture.Create<int>();
        var response = _fixture.Create<IEnumerable<DistrictResponse>>();
        A.CallTo(() => _addressService.GetDistrictsByProvinceId(A<int>._)).Returns(response);

        // Act
        var actual = await _sut.GetDistrictsByProvinceId(id);

        // Assert
        A.CallTo(() => _addressService.GetDistrictsByProvinceId(A<int>._)).MustHaveHappenedOnceExactly();
        var actionResult = Assert.IsType<OkObjectResult>(actual);
        Assert.IsType<SuccessResult<IEnumerable<DistrictResponse>>>(actionResult.Value);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetDistrictsByProvinceId_WhenThereAreNoDistrictsFound_ShouldReturnStatusCode404NotFound()
    {
        // Arrange
        var id = _fixture.Create<int>();
        var response = new List<DistrictResponse>();
        A.CallTo(() => _addressService.GetDistrictsByProvinceId(A<int>._)).Returns(response);

        // Act
        var actual = await _sut.GetDistrictsByProvinceId(id) as StatusCodeResult;

        // Assert
        A.CallTo(() => _addressService.GetDistrictsByProvinceId(A<int>._)).MustHaveHappenedOnceExactly();
        Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetDistrictsByProvinceId_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
    {
        // Arrange
        var id = _fixture.Create<int>();
        A.CallTo(() => _addressService.GetDistrictsByProvinceId(A<int>._)).Throws<Exception>();

        // Act
        var actual = await _sut.GetDistrictsByProvinceId(id) as StatusCodeResult;

        // Assert
        A.CallTo(() => _addressService.GetDistrictsByProvinceId(A<int>._)).MustHaveHappenedOnceExactly();
        A.CallTo(_logger).Where(
                call => call.Method.Name == "Log"
                && call.GetArgument<LogLevel>(0) == LogLevel.Error)
            .MustHaveHappened(1, Times.Exactly);
        Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetWardsByDistrictId_WhenThereAreWards_ShouldReturnWardsWithStatusCode200OK()
    {
        // Arrange
        var id = _fixture.Create<int>();
        var response = _fixture.Create<IEnumerable<WardResponse>>();
        A.CallTo(() => _addressService.GetWardsByDistrictId(A<int>._)).Returns(response);

        // Act
        var actual = await _sut.GetWardsByDistrictId(id);

        // Assert
        A.CallTo(() => _addressService.GetWardsByDistrictId(A<int>._)).MustHaveHappenedOnceExactly();
        var actionResult = Assert.IsType<OkObjectResult>(actual);
        Assert.IsType<SuccessResult<IEnumerable<WardResponse>>>(actionResult.Value);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetWardsByDistrictId_WhenThereAreNoWardsFound_ShouldReturnStatusCode404NotFound()
    {
        // Arrange
        var id = _fixture.Create<int>();
        var response = new List<WardResponse>();
        A.CallTo(() => _addressService.GetWardsByDistrictId(A<int>._)).Returns(response);

        // Act
        var actual = await _sut.GetWardsByDistrictId(id) as StatusCodeResult;

        // Assert
        A.CallTo(() => _addressService.GetWardsByDistrictId(A<int>._)).MustHaveHappenedOnceExactly();
        Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetWardsByDistrictId_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
    {
        // Arrange
        var id = _fixture.Create<int>();
        A.CallTo(() => _addressService.GetWardsByDistrictId(A<int>._)).Throws<Exception>();

        // Act
        var actual = await _sut.GetWardsByDistrictId(id) as StatusCodeResult;

        // Assert
        A.CallTo(() => _addressService.GetWardsByDistrictId(A<int>._)).MustHaveHappenedOnceExactly();
        A.CallTo(_logger).Where(
                call => call.Method.Name == "Log"
                && call.GetArgument<LogLevel>(0) == LogLevel.Error)
            .MustHaveHappened(1, Times.Exactly);
        Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
    }
}
