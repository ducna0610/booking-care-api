using AutoFixture;
using BookingCare.API.Controllers.Common.Wrapper;
using BookingCare.API.Controllers.V1;
using BookingCare.Application.DTOs.Requests.Doctor;
using BookingCare.Application.DTOs.Responses;
using BookingCare.Application.DTOs.Responses.Doctor;
using BookingCare.Application.Services;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookingCare.Tests.Controllers
{
    public class DoctorsControllerTests
    {
        private readonly Fixture _fixture;
        private readonly ILogger<DoctorsController> _logger;
        private readonly IDoctorService _doctorService;
        private readonly DoctorsController _sut;

        public DoctorsControllerTests()
        {
            _fixture = new Fixture();
            _logger = A.Fake<ILogger<DoctorsController>>();
            _doctorService = A.Fake<IDoctorService>();
            _sut = new DoctorsController(_logger, _doctorService);
        }

        [Fact]
        [Trait("HttpVerb", "GET")]
        public async Task ToPagination_WhenThereAreDoctors_ShouldReturnPageOfDoctorDetailsWithStatusCode200OK()
        {
            // Arrange
            var request = _fixture.Create<PaginationDoctorRequest>();
            var response = _fixture.Create<PaginationResponse<DoctorInfoDetailResponse>>();
            A.CallTo(() => _doctorService.ToPagination(A<PaginationDoctorRequest>._)).Returns(response);

            // Act
            var actual = await _sut.ToPagination(request);

            // Assert
            A.CallTo(() => _doctorService.ToPagination(A<PaginationDoctorRequest>._)).MustHaveHappenedOnceExactly();
            var actionResult = Assert.IsType<OkObjectResult>(actual);
            Assert.IsType<SuccessResult<PaginationResponse<DoctorInfoDetailResponse>>>(actionResult.Value);
        }

        [Fact]
        [Trait("HttpVerb", "GET")]
        public async Task ToPagination_WhenThereAreNoDoctorsFound_ShouldReturnStatusCode404NotFound()
        {
            // Arrange
            var request = _fixture.Create<PaginationDoctorRequest>();
            var response = new PaginationResponse<DoctorInfoDetailResponse>();
            A.CallTo(() => _doctorService.ToPagination(A<PaginationDoctorRequest>._)).Returns(response);

            // Act
            var actual = await _sut.ToPagination(request) as StatusCodeResult;

            // Assert
            A.CallTo(() => _doctorService.ToPagination(A<PaginationDoctorRequest>._)).MustHaveHappenedOnceExactly();
            Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
        }

        [Fact]
        [Trait("HttpVerb", "GET")]
        public async Task ToPagination_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
        {
            // Arrange
            var request = _fixture.Create<PaginationDoctorRequest>();
            A.CallTo(() => _doctorService.ToPagination(A<PaginationDoctorRequest>._)).Throws<Exception>();

            // Act
            var actual = await _sut.ToPagination(request) as StatusCodeResult;

            // Assert
            A.CallTo(() => _doctorService.ToPagination(A<PaginationDoctorRequest>._)).MustHaveHappenedOnceExactly();
            A.CallTo(_logger).Where(
                    call => call.Method.Name == "Log"
                    && call.GetArgument<LogLevel>(0) == LogLevel.Error)
                .MustHaveHappened(1, Times.Exactly);
            Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
        }

        [Fact]
        [Trait("HttpVerb", "GET")]
        public async Task GetById_WhenThereIsDoctor_ShouldReturnDoctorWithStatusCode200OK()
        {
            // Arrange
            var request = _fixture.Create<Guid>();
            var response = _fixture.Create<DoctorInfoDetailResponse>();
            A.CallTo(() => _doctorService.GetById(A<Guid>._)).Returns(response);

            // Act
            var actual = await _sut.GetById(request);

            // Assert
            A.CallTo(() => _doctorService.GetById(A<Guid>._)).MustHaveHappenedOnceExactly();
            var actionResult = Assert.IsType<OkObjectResult>(actual);
            Assert.IsType<SuccessResult<DoctorInfoDetailResponse>>(actionResult.Value);
        }

        [Fact]
        [Trait("HttpVerb", "GET")]
        public async Task GetById_WhenThereAreNoDoctorFound_ShouldReturnStatusCode404NotFound()
        {
            // Arrange
            var request = _fixture.Create<Guid>();
            var response = new DoctorInfoDetailResponse();
            A.CallTo(() => _doctorService.GetById(A<Guid>._)).Returns(response);

            // Act
            var actual = await _sut.GetById(request) as StatusCodeResult;

            // Assert
            A.CallTo(() => _doctorService.GetById(A<Guid>._)).MustHaveHappenedOnceExactly();
            Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
        }

        [Fact]
        [Trait("HttpVerb", "GET")]
        public async Task GetById_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
        {
            // Arrange
            var id = _fixture.Create<Guid>();
            A.CallTo(() => _doctorService.GetById(A<Guid>._)).Throws<Exception>();

            // Act
            var actual = await _sut.GetById(id) as StatusCodeResult;

            // Assert
            A.CallTo(() => _doctorService.GetById(A<Guid>._)).MustHaveHappenedOnceExactly();
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
            var request = _fixture.Create<CreateDoctorRequest>();
            var response = _fixture.Create<DoctorInfoResponse>();
            A.CallTo(() => _doctorService.Create(A<CreateDoctorRequest>._)).Returns(response);

            // Act
            var actual = await _sut.Create(request);

            // Assert
            A.CallTo(() => _doctorService.Create(A<CreateDoctorRequest>._)).MustHaveHappenedOnceExactly();
            var actionResult = Assert.IsType<CreatedAtActionResult>(actual);
            Assert.IsType<SuccessResult<DoctorInfoResponse>>(actionResult.Value);
        }

        [Fact]
        [Trait("HttpVerb", "POST")]
        public async Task Create_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
        {
            // Arrange
            _fixture.Register<IFormFile>(() => null);
            var request = _fixture.Create<CreateDoctorRequest>();
            A.CallTo(() => _doctorService.Create(A<CreateDoctorRequest>._)).Throws<Exception>();

            // Act
            var actual = await _sut.Create(request) as StatusCodeResult;

            // Assert
            A.CallTo(() => _doctorService.Create(A<CreateDoctorRequest>._)).MustHaveHappenedOnceExactly();
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
            var request = _fixture.Create<UpdateDoctorRequest>();
            var response = _fixture.Create<DoctorInfoResponse>();
            A.CallTo(() => _doctorService.Update(A<Guid>._, A<UpdateDoctorRequest>._)).Returns(response);

            // Act
            var actual = await _sut.Update(id, request);

            // Assert
            A.CallTo(() => _doctorService.Update(A<Guid>._, A<UpdateDoctorRequest>._)).MustHaveHappenedOnceExactly();
            var actionResult = Assert.IsType<OkObjectResult>(actual);
            Assert.IsType<SuccessResult<DoctorInfoResponse>>(actionResult.Value);
        }

        [Fact]
        [Trait("HttpVerb", "PUT")]
        public async Task Update_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
        {
            // Arrange
            _fixture.Register<IFormFile>(() => null);
            var id = _fixture.Create<Guid>();
            var request = _fixture.Create<UpdateDoctorRequest>();
            A.CallTo(() => _doctorService.Update(A<Guid>._, A<UpdateDoctorRequest>._)).Throws<Exception>();

            // Act
            var actual = await _sut.Update(id, request) as StatusCodeResult;

            // Assert
            A.CallTo(() => _doctorService.Update(A<Guid>._, A<UpdateDoctorRequest>._)).MustHaveHappenedOnceExactly();
            A.CallTo(_logger).Where(
                    call => call.Method.Name == "Log"
                    && call.GetArgument<LogLevel>(0) == LogLevel.Error)
                .MustHaveHappened(1, Times.Exactly);
            Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
        }

        [Fact]
        [Trait("HttpVerb", "GET")]
        public async Task GetSchedule_WhenThereAreSchedules_ShouldReturnSchedulesDetailsWithStatusCode200OK()
        {
            // Arrange
            var request = _fixture.Create<GetScheduleRequest>();
            var response = _fixture.CreateMany<ScheduleResponse>(3).ToList();
            A.CallTo(() => _doctorService.GetSchedule(A<GetScheduleRequest>._)).Returns(response);

            // Act
            var actual = await _sut.GetSchedule(request);

            // Assert
            A.CallTo(() => _doctorService.GetSchedule(A<GetScheduleRequest>._)).MustHaveHappenedOnceExactly();
            var actionResult = Assert.IsType<OkObjectResult>(actual);
            var result = Assert.IsType<SuccessResult<IEnumerable<ScheduleResponse>>>(actionResult.Value);
            Assert.Equal(response.Count(), result.Data.Count());
        }

        [Fact]
        [Trait("HttpVerb", "GET")]
        public async Task GetSchedule_WhenThereAreNoSchedulesFound_ShouldReturnStatusCode404NotFound()
        {
            // Arrange
            var request = _fixture.Create<GetScheduleRequest>();
            var response = new List<ScheduleResponse>();
            A.CallTo(() => _doctorService.GetSchedule(A<GetScheduleRequest>._)).Returns(response);

            // Act
            var actual = await _sut.GetSchedule(request) as StatusCodeResult;

            // Assert
            A.CallTo(() => _doctorService.GetSchedule(A<GetScheduleRequest>._)).MustHaveHappenedOnceExactly();
            Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
        }

        [Fact]
        [Trait("HttpVerb", "GET")]
        public async Task GetSchedule_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
        {
            // Arrange
            var request = _fixture.Create<GetScheduleRequest>();
            A.CallTo(() => _doctorService.GetSchedule(A<GetScheduleRequest>._)).Throws<Exception>();

            // Act
            var actual = await _sut.GetSchedule(request) as StatusCodeResult;

            // Assert
            A.CallTo(() => _doctorService.GetSchedule(A<GetScheduleRequest>._)).MustHaveHappenedOnceExactly();
            A.CallTo(_logger).Where(
                    call => call.Method.Name == "Log"
                    && call.GetArgument<LogLevel>(0) == LogLevel.Error)
                .MustHaveHappened(1, Times.Exactly);
            Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
        }

        [Fact]
        [Trait("HttpVerb", "PUT")]
        public async Task SetSchedule_WhenValid_ShouldReturnStatusCode201Created()
        {
            // Arrange
            var request = _fixture.Create<SetScheduleRequest>();
            var response = _fixture.CreateMany<ScheduleResponse>(3).ToList();
            A.CallTo(() => _doctorService.SetSchedule(A<SetScheduleRequest>._)).Returns(response);

            // Act
            var actual = await _sut.SetSchedule(request);

            // Assert
            A.CallTo(() => _doctorService.SetSchedule(A<SetScheduleRequest>._)).MustHaveHappenedOnceExactly();
            var actionResult = Assert.IsType<OkObjectResult>(actual);
            var result = Assert.IsType<SuccessResult<IEnumerable<ScheduleResponse>>>(actionResult.Value);
            Assert.Equal(response.Count(), result.Data.Count());
        }

        [Fact]
        [Trait("HttpVerb", "PUT")]
        public async Task SetSchedule_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
        {
            // Arrange
            var request = _fixture.Create<SetScheduleRequest>();
            A.CallTo(() => _doctorService.SetSchedule(A<SetScheduleRequest>._)).Throws<Exception>();

            // Act
            var actual = await _sut.SetSchedule(request) as StatusCodeResult;

            // Assert
            A.CallTo(() => _doctorService.SetSchedule(A<SetScheduleRequest>._)).MustHaveHappenedOnceExactly();
            A.CallTo(_logger).Where(
                    call => call.Method.Name == "Log"
                    && call.GetArgument<LogLevel>(0) == LogLevel.Error)
                .MustHaveHappened(1, Times.Exactly);
            Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
        }
    }
}
