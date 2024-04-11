using AutoFixture;
using BookingCare.API.Controllers.Common.Wrapper;
using BookingCare.API.Controllers.V1;
using BookingCare.Application.DTOs.Requests.Clinic;
using BookingCare.Application.DTOs.Responses;
using BookingCare.Application.DTOs.Responses.Clinic;
using BookingCare.Application.Services;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookingCare.Tests.Controllers;

public class ClinicsControllerTests
{
    private readonly Fixture _fixture;
    private readonly ILogger<ClinicsController> _logger;
    private readonly IClinicService _clinicService;
    private readonly ClinicsController _sut;

    public ClinicsControllerTests()
    {
        _fixture = new Fixture();
        _logger = A.Fake<ILogger<ClinicsController>>();
        _clinicService = A.Fake<IClinicService>();
        _sut = new ClinicsController(_logger, _clinicService);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task ToPagination_WhenThereAreClinics_ShouldReturnPageOfClinicDetailsWithStatusCode200OK()
    {
        // Arrange
        var request = _fixture.Create<PaginationClinicRequest>();
        var response = _fixture.Create<PaginationResponse<ClinicDetailResponse>>();
        A.CallTo(() => _clinicService.ToPagination(A<PaginationClinicRequest>._)).Returns(response);

        // Act
        var actual = await _sut.ToPagination(request);

        // Assert
        A.CallTo(() => _clinicService.ToPagination(A<PaginationClinicRequest>._)).MustHaveHappenedOnceExactly();
        var actionResult = Assert.IsType<OkObjectResult>(actual);
        Assert.IsType<SuccessResult<PaginationResponse<ClinicDetailResponse>>>(actionResult.Value);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task ToPagination_WhenThereAreNoClinicsFound_ShouldReturnStatusCode404NotFound()
    {
        // Arrange
        var request = _fixture.Create<PaginationClinicRequest>();
        var response = new PaginationResponse<ClinicDetailResponse>();
        A.CallTo(() => _clinicService.ToPagination(A<PaginationClinicRequest>._)).Returns(response);

        // Act
        var actual = await _sut.ToPagination(request) as StatusCodeResult;

        // Assert
        A.CallTo(() => _clinicService.ToPagination(A<PaginationClinicRequest>._)).MustHaveHappenedOnceExactly();
        Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task ToPagination_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
    {
        // Arrange
        var request = _fixture.Create<PaginationClinicRequest>();
        A.CallTo(() => _clinicService.ToPagination(A<PaginationClinicRequest>._)).Throws<Exception>();

        // Act
        var actual = await _sut.ToPagination(request) as StatusCodeResult;

        // Assert
        A.CallTo(() => _clinicService.ToPagination(A<PaginationClinicRequest>._)).MustHaveHappenedOnceExactly();
        A.CallTo(_logger).Where(
                call => call.Method.Name == "Log"
                && call.GetArgument<LogLevel>(0) == LogLevel.Error)
            .MustHaveHappened(1, Times.Exactly);
        Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetListName_WhenThereAreClinics_ShouldReturnLisNameClinicsWithStatusCode200OK()
    {
        // Arrange
        var response = _fixture.CreateMany<NameResponse>(3).ToList();
        A.CallTo(() => _clinicService.GetListName()).Returns(response);

        // Act
        var actual = await _sut.GetListName();

        // Assert
        A.CallTo(() => _clinicService.GetListName()).MustHaveHappenedOnceExactly();
        var actionResult = Assert.IsType<OkObjectResult>(actual);
        var result = Assert.IsType<SuccessResult<IEnumerable<NameResponse>>>(actionResult.Value);
        Assert.Equal(response.Count(), result.Data.Count());
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetListName_WhenThereAreNoClinicsFound_ShouldReturnStatusCode404NotFound()
    {
        // Arrange
        var response = new List<NameResponse>();
        A.CallTo(() => _clinicService.GetListName()).Returns(response);

        // Act
        var actual = await _sut.GetListName() as StatusCodeResult;

        // Assert
        A.CallTo(() => _clinicService.GetListName()).MustHaveHappenedOnceExactly();
        Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetListName_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
    {
        // Arrange
        A.CallTo(() => _clinicService.GetListName()).Throws<Exception>();

        // Act
        var actual = await _sut.GetListName() as StatusCodeResult;

        // Assert
        A.CallTo(() => _clinicService.GetListName()).MustHaveHappenedOnceExactly();
        A.CallTo(_logger).Where(
                call => call.Method.Name == "Log"
                && call.GetArgument<LogLevel>(0) == LogLevel.Error)
            .MustHaveHappened(1, Times.Exactly);
        Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetById_WhenThereIsClinic_ShouldReturnClinicWithStatusCode200OK()
    {
        // Arrange
        var request = _fixture.Create<Guid>();
        var response = _fixture.Create<ClinicDetailResponse>();
        A.CallTo(() => _clinicService.GetById(A<Guid>._)).Returns(response);

        // Act
        var actual = await _sut.GetById(request);

        // Assert
        A.CallTo(() => _clinicService.GetById(A<Guid>._)).MustHaveHappenedOnceExactly();
        var actionResult = Assert.IsType<OkObjectResult>(actual);
        Assert.IsType<SuccessResult<ClinicDetailResponse>>(actionResult.Value);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetById_WhenThereAreNoClinicFound_ShouldReturnStatusCode404NotFound()
    {
        // Arrange
        var request = _fixture.Create<Guid>();
        var response = new ClinicDetailResponse();
        A.CallTo(() => _clinicService.GetById(A<Guid>._)).Returns(response);

        // Act
        var actual = await _sut.GetById(request) as StatusCodeResult;

        // Assert
        A.CallTo(() => _clinicService.GetById(A<Guid>._)).MustHaveHappenedOnceExactly();
        Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetById_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        A.CallTo(() => _clinicService.GetById(A<Guid>._)).Throws<Exception>();

        // Act
        var actual = await _sut.GetById(id) as StatusCodeResult;

        // Assert
        A.CallTo(() => _clinicService.GetById(A<Guid>._)).MustHaveHappenedOnceExactly();
        A.CallTo(_logger).Where(
                call => call.Method.Name == "Log"
                && call.GetArgument<LogLevel>(0) == LogLevel.Error)
            .MustHaveHappened(1, Times.Exactly);
        Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetByIdForAdmin_WhenThereIsClinic_ShouldReturnClinicWithStatusCode200OK()
    {
        // Arrange
        var request = _fixture.Create<Guid>();
        var response = _fixture.Create<ClinicDetailForAdminResponse>();
        A.CallTo(() => _clinicService.GetByIdForAdmin(A<Guid>._)).Returns(response);

        // Act
        var actual = await _sut.GetByIdForAdmin(request);

        // Assert
        A.CallTo(() => _clinicService.GetByIdForAdmin(A<Guid>._)).MustHaveHappenedOnceExactly();
        var actionResult = Assert.IsType<OkObjectResult>(actual);
        Assert.IsType<SuccessResult<ClinicDetailForAdminResponse>>(actionResult.Value);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetByIdForAdmin_WhenThereAreNoClinicFound_ShouldReturnStatusCode404NotFound()
    {
        // Arrange
        var request = _fixture.Create<Guid>();
        var response = new ClinicDetailForAdminResponse();
        A.CallTo(() => _clinicService.GetByIdForAdmin(A<Guid>._)).Returns(response);

        // Act
        var actual = await _sut.GetByIdForAdmin(request) as StatusCodeResult;

        // Assert
        A.CallTo(() => _clinicService.GetByIdForAdmin(A<Guid>._)).MustHaveHappenedOnceExactly();
        Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetByIdForAdmin_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        A.CallTo(() => _clinicService.GetByIdForAdmin(A<Guid>._)).Throws<Exception>();

        // Act
        var actual = await _sut.GetByIdForAdmin(id) as StatusCodeResult;

        // Assert
        A.CallTo(() => _clinicService.GetByIdForAdmin(A<Guid>._)).MustHaveHappenedOnceExactly();
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
        var request = _fixture.Create<CreateClinicRequest>();
        var response = _fixture.Create<ClinicResponse>();
        A.CallTo(() => _clinicService.Create(A<CreateClinicRequest>._)).Returns(response);

        // Act
        var actual = await _sut.Create(request);

        // Assert
        A.CallTo(() => _clinicService.Create(A<CreateClinicRequest>._)).MustHaveHappenedOnceExactly();
        var actionResult = Assert.IsType<CreatedAtActionResult>(actual);
        Assert.IsType<SuccessResult<ClinicResponse>>(actionResult.Value);
    }

    [Fact]
    [Trait("HttpVerb", "POST")]
    public async Task Create_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
    {
        // Arrange
        _fixture.Register<IFormFile>(() => null);
        var request = _fixture.Create<CreateClinicRequest>();
        A.CallTo(() => _clinicService.Create(A<CreateClinicRequest>._)).Throws<Exception>();

        // Act
        var actual = await _sut.Create(request) as StatusCodeResult;

        // Assert
        A.CallTo(() => _clinicService.Create(A<CreateClinicRequest>._)).MustHaveHappenedOnceExactly();
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
        _fixture.Register<IFormFile>(() => null);
        var id = _fixture.Create<Guid>();
        var request = _fixture.Create<UpdateClinicRequest>();
        var response = _fixture.Create<ClinicResponse>();
        A.CallTo(() => _clinicService.Update(A<Guid>._, A<UpdateClinicRequest>._)).Returns(response);

        // Act
        var actual = await _sut.Update(id, request);

        // Assert
        A.CallTo(() => _clinicService.Update(A<Guid>._, A<UpdateClinicRequest>._)).MustHaveHappenedOnceExactly();
        var actionResult = Assert.IsType<OkObjectResult>(actual);
        Assert.IsType<SuccessResult<ClinicResponse>>(actionResult.Value);
    }

    [Fact]
    [Trait("HttpVerb", "PUT")]
    public async Task Update_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
    {
        // Arrange
        _fixture.Register<IFormFile>(() => null);
        var id = _fixture.Create<Guid>();
        var request = _fixture.Create<UpdateClinicRequest>();
        A.CallTo(() => _clinicService.Update(A<Guid>._, A<UpdateClinicRequest>._)).Throws<Exception>();

        // Act
        var actual = await _sut.Update(id, request) as StatusCodeResult;

        // Assert
        A.CallTo(() => _clinicService.Update(A<Guid>._, A<UpdateClinicRequest>._)).MustHaveHappenedOnceExactly();
        A.CallTo(_logger).Where(
                call => call.Method.Name == "Log"
                && call.GetArgument<LogLevel>(0) == LogLevel.Error)
            .MustHaveHappened(1, Times.Exactly);
        Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
    }

    [Fact]
    [Trait("HttpVerb", "DELETE")]
    public async Task Delete_WhenValid_ShouldReturnStatusCode204NoContent()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        A.CallTo(() => _clinicService.Delete(A<Guid>._)).Returns(Task.CompletedTask);

        // Act
        var actual = await _sut.Delete(id);

        // Assert
        A.CallTo(() => _clinicService.Delete(A<Guid>._)).MustHaveHappenedOnceExactly();
        var actionResult = Assert.IsType<NoContentResult>(actual);
    }

    [Fact]
    [Trait("HttpVerb", "DELETE")]
    public async Task Delete_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        A.CallTo(() => _clinicService.Delete(A<Guid>._)).Throws<Exception>();

        // Act
        var actual = await _sut.Delete(id) as StatusCodeResult;

        // Assert
        A.CallTo(() => _clinicService.Delete(A<Guid>._)).MustHaveHappenedOnceExactly();
        A.CallTo(_logger).Where(
                call => call.Method.Name == "Log"
                && call.GetArgument<LogLevel>(0) == LogLevel.Error)
            .MustHaveHappened(1, Times.Exactly);
        Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
    }

    [Fact]
    [Trait("HttpVerb", "POST")]
    public async Task Import_WhenValid_ShouldReturnStatusCode200OK()
    {
        // Arrange
        _fixture.Register<IFormFile>(() => null);
        var file = _fixture.Create<IFormFile>();
        var response = _fixture.CreateMany<ClinicResponse>(3).ToList();
        A.CallTo(() => _clinicService.Import(A<IFormFile>._)).Returns(response);

        // Act
        var actual = await _sut.Import(file);

        // Assert
        A.CallTo(() => _clinicService.Import(A<IFormFile>._)).MustHaveHappenedOnceExactly();
        var actionResult = Assert.IsType<OkObjectResult>(actual);
        var result = Assert.IsType<SuccessResult<IEnumerable<ClinicResponse>>>(actionResult.Value);
        Assert.Equal(response.Count(), result.Data.Count());
    }

    [Fact]
    [Trait("HttpVerb", "POST")]
    public async Task Import_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
    {
        // Arrange
        _fixture.Register<IFormFile>(() => null);
        var file = _fixture.Create<IFormFile>();
        A.CallTo(() => _clinicService.Import(A<IFormFile>._)).Throws<Exception>();

        // Act
        var actual = await _sut.Import(file) as StatusCodeResult;

        // Assert
        A.CallTo(() => _clinicService.Import(A<IFormFile>._)).MustHaveHappenedOnceExactly();
        A.CallTo(_logger).Where(
                call => call.Method.Name == "Log"
                && call.GetArgument<LogLevel>(0) == LogLevel.Error)
            .MustHaveHappened(1, Times.Exactly);
        Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
    }

    [Fact]
    [Trait("HttpVerb", "POST")]
    public async Task Export_WhenValid_ShouldReturnStatusCode200OK()
    {
        // Arrange
        var response = _fixture.Create<string>();
        A.CallTo(() => _clinicService.Export()).Returns(response);

        // Act
        var actual = await _sut.Export();

        // Assert
        A.CallTo(() => _clinicService.Export()).MustHaveHappenedOnceExactly();
        var actionResult = Assert.IsType<OkObjectResult>(actual);
        Assert.IsType<SuccessResult<string>>(actionResult.Value);
    }

    [Fact]
    [Trait("HttpVerb", "POST")]
    public async Task Export_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
    {
        // Arrange
        A.CallTo(() => _clinicService.Export()).Throws<Exception>();

        // Act
        var actual = await _sut.Export() as StatusCodeResult;

        // Assert
        A.CallTo(() => _clinicService.Export()).MustHaveHappenedOnceExactly();
        A.CallTo(_logger).Where(
                call => call.Method.Name == "Log"
                && call.GetArgument<LogLevel>(0) == LogLevel.Error)
            .MustHaveHappened(1, Times.Exactly);
        Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
    }
}
