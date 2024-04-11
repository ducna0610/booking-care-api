using AutoFixture;
using BookingCare.API.Controllers.Common.Wrapper;
using BookingCare.API.Controllers.V1;
using BookingCare.Application.DTOs.Requests.Speciality;
using BookingCare.Application.DTOs.Responses;
using BookingCare.Application.DTOs.Responses.Speciality;
using BookingCare.Application.Services;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookingCare.Tests.Controllers;

public class SpecialitiesControllerTests
{
    private readonly Fixture _fixture;
    private readonly ILogger<SpecialitiesController> _logger;
    private readonly ISpecialityService _specialityService;
    private readonly SpecialitiesController _sut;

    public SpecialitiesControllerTests()
    {
        _fixture = new Fixture();
        _logger = A.Fake<ILogger<SpecialitiesController>>();
        _specialityService = A.Fake<ISpecialityService>();
        _sut = new SpecialitiesController(_logger, _specialityService);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task ToPagination_WhenThereAreSpecialities_ShouldReturnPageOfSpecialityDetailsWithStatusCode200OK()
    {
        // Arrange
        var request = _fixture.Create<PaginationSpecialityRequest>();
        var response = _fixture.Create<PaginationResponse<SpecialityDetailResponse>>();
        A.CallTo(() => _specialityService.ToPagination(A<PaginationSpecialityRequest>._)).Returns(response);

        // Act
        var actual = await _sut.ToPagination(request);

        // Assert
        A.CallTo(() => _specialityService.ToPagination(A<PaginationSpecialityRequest>._)).MustHaveHappenedOnceExactly();
        var actionResult = Assert.IsType<OkObjectResult>(actual);
        Assert.IsType<SuccessResult<PaginationResponse<SpecialityDetailResponse>>>(actionResult.Value);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task ToPagination_WhenThereAreNoSpecialitiesFound_ShouldReturnStatusCode404NotFound()
    {
        // Arrange
        var request = _fixture.Create<PaginationSpecialityRequest>();
        var response = new PaginationResponse<SpecialityDetailResponse>();
        A.CallTo(() => _specialityService.ToPagination(A<PaginationSpecialityRequest>._)).Returns(response);

        // Act
        var actual = await _sut.ToPagination(request) as StatusCodeResult;

        // Assert
        A.CallTo(() => _specialityService.ToPagination(A<PaginationSpecialityRequest>._)).MustHaveHappenedOnceExactly();
        Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task ToPagination_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
    {
        // Arrange
        var request = _fixture.Create<PaginationSpecialityRequest>();
        A.CallTo(() => _specialityService.ToPagination(A<PaginationSpecialityRequest>._)).Throws<Exception>();

        // Act
        var actual = await _sut.ToPagination(request) as StatusCodeResult;

        // Assert
        A.CallTo(() => _specialityService.ToPagination(A<PaginationSpecialityRequest>._)).MustHaveHappenedOnceExactly();
        A.CallTo(_logger).Where(
                call => call.Method.Name == "Log"
                && call.GetArgument<LogLevel>(0) == LogLevel.Error)
            .MustHaveHappened(1, Times.Exactly);
        Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetAll_WhenThereAreSpecialities_ShouldReturnListSpecialitiesWithStatusCode200OK()
    {
        // Arrange
        var response = _fixture.CreateMany<SpecialityDetailResponse>(3).ToList();
        A.CallTo(() => _specialityService.GetAll()).Returns(response);

        // Act
        var actual = await _sut.GetAll();

        // Assert
        A.CallTo(() => _specialityService.GetAll()).MustHaveHappenedOnceExactly();
        var actionResult = Assert.IsType<OkObjectResult>(actual);
        var result = Assert.IsType<SuccessResult<IEnumerable<SpecialityDetailResponse>>>(actionResult.Value);
        Assert.Equal(response.Count(), result.Data.Count());
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetAll_WhenThereAreNoSpecialitiesFound_ShouldReturnStatusCode404NotFound()
    {
        // Arrange
        var response = new List<SpecialityDetailResponse>();
        A.CallTo(() => _specialityService.GetAll()).Returns(response);

        // Act
        var actual = await _sut.GetAll() as StatusCodeResult;

        // Assert
        A.CallTo(() => _specialityService.GetAll()).MustHaveHappenedOnceExactly();
        Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetAll_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
    {
        // Arrange
        A.CallTo(() => _specialityService.GetAll()).Throws<Exception>();

        // Act
        var actual = await _sut.GetAll() as StatusCodeResult;

        // Assert
        A.CallTo(() => _specialityService.GetAll()).MustHaveHappenedOnceExactly();
        A.CallTo(_logger).Where(
                call => call.Method.Name == "Log"
                && call.GetArgument<LogLevel>(0) == LogLevel.Error)
            .MustHaveHappened(1, Times.Exactly);
        Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetListName_WhenThereAreSpecialities_ShouldReturnListNameSpecialitiesWithStatusCode200OK()
    {
        // Arrange
        var response = _fixture.CreateMany<NameResponse>(3).ToList();
        A.CallTo(() => _specialityService.GetListName()).Returns(response);

        // Act
        var actual = await _sut.GetListName();

        // Assert
        A.CallTo(() => _specialityService.GetListName()).MustHaveHappenedOnceExactly();
        var actionResult = Assert.IsType<OkObjectResult>(actual);
        var result = Assert.IsType<SuccessResult<IEnumerable<NameResponse>>>(actionResult.Value);
        Assert.Equal(response.Count(), result.Data.Count());
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetListName_WhenThereAreNoSpecialitiesFound_ShouldReturnStatusCode404NotFound()
    {
        // Arrange
        var response = new List<NameResponse>();
        A.CallTo(() => _specialityService.GetListName()).Returns(response);

        // Act
        var actual = await _sut.GetListName() as StatusCodeResult;

        // Assert
        A.CallTo(() => _specialityService.GetListName()).MustHaveHappenedOnceExactly();
        Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetListName_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
    {
        // Arrange
        A.CallTo(() => _specialityService.GetListName()).Throws<Exception>();

        // Act
        var actual = await _sut.GetListName() as StatusCodeResult;

        // Assert
        A.CallTo(() => _specialityService.GetListName()).MustHaveHappenedOnceExactly();
        A.CallTo(_logger).Where(
                call => call.Method.Name == "Log"
                && call.GetArgument<LogLevel>(0) == LogLevel.Error)
            .MustHaveHappened(1, Times.Exactly);
        Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetById_WhenThereIsSpeciality_ShouldReturnSpecialityWithStatusCode200OK()
    {
        // Arrange
        var request = _fixture.Create<Guid>();
        var response = _fixture.Create<SpecialityResponse>();
        A.CallTo(() => _specialityService.GetById(A<Guid>._)).Returns(response);

        // Act
        var actual = await _sut.GetById(request);

        // Assert
        A.CallTo(() => _specialityService.GetById(A<Guid>._)).MustHaveHappenedOnceExactly();
        var actionResult = Assert.IsType<OkObjectResult>(actual);
        Assert.IsType<SuccessResult<SpecialityResponse>>(actionResult.Value);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetById_WhenThereAreNoSpecialityFound_ShouldReturnStatusCode404NotFound()
    {
        // Arrange
        var request = _fixture.Create<Guid>();
        var response = new SpecialityResponse();
        A.CallTo(() => _specialityService.GetById(A<Guid>._)).Returns(response);

        // Act
        var actual = await _sut.GetById(request) as StatusCodeResult;

        // Assert
        A.CallTo(() => _specialityService.GetById(A<Guid>._)).MustHaveHappenedOnceExactly();
        Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
    }

    [Fact]
    [Trait("HttpVerb", "GET")]
    public async Task GetById_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        A.CallTo(() => _specialityService.GetById(A<Guid>._)).Throws<Exception>();

        // Act
        var actual = await _sut.GetById(id) as StatusCodeResult;

        // Assert
        A.CallTo(() => _specialityService.GetById(A<Guid>._)).MustHaveHappenedOnceExactly();
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
        var request = _fixture.Create<CreateSpecialityRequest>();
        var response = _fixture.Create<SpecialityResponse>();
        A.CallTo(() => _specialityService.Create(A<CreateSpecialityRequest>._)).Returns(response);

        // Act
        var actual = await _sut.Create(request);

        // Assert
        A.CallTo(() => _specialityService.Create(A<CreateSpecialityRequest>._)).MustHaveHappenedOnceExactly();
        var actionResult = Assert.IsType<CreatedAtActionResult>(actual);
        Assert.IsType<SuccessResult<SpecialityResponse>>(actionResult.Value);
    }

    [Fact]
    [Trait("HttpVerb", "POST")]
    public async Task Create_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
    {
        // Arrange
        _fixture.Register<IFormFile>(() => null);
        var request = _fixture.Create<CreateSpecialityRequest>();
        A.CallTo(() => _specialityService.Create(A<CreateSpecialityRequest>._)).Throws<Exception>();

        // Act
        var actual = await _sut.Create(request) as StatusCodeResult;

        // Assert
        A.CallTo(() => _specialityService.Create(A<CreateSpecialityRequest>._)).MustHaveHappenedOnceExactly();
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
        var request = _fixture.Create<UpdateSpecialityRequest>();
        var response = _fixture.Create<SpecialityResponse>();
        A.CallTo(() => _specialityService.Update(A<Guid>._, A<UpdateSpecialityRequest>._)).Returns(response);

        // Act
        var actual = await _sut.Update(id, request);

        // Assert
        A.CallTo(() => _specialityService.Update(A<Guid>._, A<UpdateSpecialityRequest>._)).MustHaveHappenedOnceExactly();
        var actionResult = Assert.IsType<OkObjectResult>(actual);
        Assert.IsType<SuccessResult<SpecialityResponse>>(actionResult.Value);
    }

    [Fact]
    [Trait("HttpVerb", "PUT")]
    public async Task Update_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
    {
        // Arrange
        _fixture.Register<IFormFile>(() => null);
        var id = _fixture.Create<Guid>();
        var request = _fixture.Create<UpdateSpecialityRequest>();
        A.CallTo(() => _specialityService.Update(A<Guid>._, A<UpdateSpecialityRequest>._)).Throws<Exception>();

        // Act
        var actual = await _sut.Update(id, request) as StatusCodeResult;

        // Assert
        A.CallTo(() => _specialityService.Update(A<Guid>._, A<UpdateSpecialityRequest>._)).MustHaveHappenedOnceExactly();
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
        A.CallTo(() => _specialityService.Delete(A<Guid>._)).Returns(Task.CompletedTask);

        // Act
        var actual = await _sut.Delete(id);

        // Assert
        A.CallTo(() => _specialityService.Delete(A<Guid>._)).MustHaveHappenedOnceExactly();
        var actionResult = Assert.IsType<NoContentResult>(actual);
    }

    [Fact]
    [Trait("HttpVerb", "DELETE")]
    public async Task Delete_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        A.CallTo(() => _specialityService.Delete(A<Guid>._)).Throws<Exception>();

        // Act
        var actual = await _sut.Delete(id) as StatusCodeResult;

        // Assert
        A.CallTo(() => _specialityService.Delete(A<Guid>._)).MustHaveHappenedOnceExactly();
        A.CallTo(_logger).Where(
                call => call.Method.Name == "Log"
                && call.GetArgument<LogLevel>(0) == LogLevel.Error)
            .MustHaveHappened(1, Times.Exactly);
        Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
    }

}
