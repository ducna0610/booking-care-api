using AutoFixture;
using BookingCare.API.Controllers.Common.Wrapper;
using BookingCare.API.Controllers.V1;
using BookingCare.Application.DTOs.Requests;
using BookingCare.Application.DTOs.Requests.Contact;
using BookingCare.Application.DTOs.Responses;
using BookingCare.Application.DTOs.Responses.Contact;
using BookingCare.Application.Services;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookingCare.Tests.Controllers
{
    public class ContactsControllerTests
    {
        private readonly Fixture _fixture;
        private readonly ILogger<ClinicsController> _logger;
        private readonly IContactService _contactService;
        private readonly ContactsController _sut;

        public ContactsControllerTests()
        {
            _fixture = new Fixture();
            _logger = A.Fake<ILogger<ClinicsController>>();
            _contactService = A.Fake<IContactService>();
            _sut = new ContactsController(_logger, _contactService);
        }

        [Fact]
        [Trait("HttpVerb", "GET")]
        public async Task ToPagination_WhenThereAreContacts_ShouldReturnPageOfClinicDetailsWithStatusCode200OK()
        {
            // Arrange
            var request = _fixture.Create<PaginationRequest>();
            var response = _fixture.Create<PaginationResponse<ContactResponse>>();
            A.CallTo(() => _contactService.ToPagination(A<PaginationRequest>._)).Returns(response);

            // Act
            var actual = await _sut.ToPagination(request);

            // Assert
            A.CallTo(() => _contactService.ToPagination(A<PaginationRequest>._)).MustHaveHappenedOnceExactly();
            var actionResult = Assert.IsType<OkObjectResult>(actual);
            Assert.IsType<SuccessResult<PaginationResponse<ContactResponse>>>(actionResult.Value);
        }

        [Fact]
        [Trait("HttpVerb", "GET")]
        public async Task ToPagination_WhenThereAreNoContactsFound_ShouldReturnStatusCode404NotFound()
        {
            // Arrange
            var request = _fixture.Create<PaginationRequest>();
            var response = new PaginationResponse<ContactResponse>();
            A.CallTo(() => _contactService.ToPagination(A<PaginationRequest>._)).Returns(response);

            // Act
            var actual = await _sut.ToPagination(request) as StatusCodeResult;

            // Assert
            A.CallTo(() => _contactService.ToPagination(A<PaginationRequest>._)).MustHaveHappenedOnceExactly();
            Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
        }

        [Fact]
        [Trait("HttpVerb", "GET")]
        public async Task ToPagination_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
        {
            // Arrange
            var request = _fixture.Create<PaginationRequest>();
            A.CallTo(() => _contactService.ToPagination(A<PaginationRequest>._)).Throws<Exception>();

            // Act
            var actual = await _sut.ToPagination(request) as StatusCodeResult;

            // Assert
            A.CallTo(() => _contactService.ToPagination(A<PaginationRequest>._)).MustHaveHappenedOnceExactly();
            A.CallTo(_logger).Where(
                    call => call.Method.Name == "Log"
                    && call.GetArgument<LogLevel>(0) == LogLevel.Error)
                .MustHaveHappened(1, Times.Exactly);
            Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
        }

        [Fact]
        [Trait("HttpVerb", "GET")]
        public async Task GetById_WhenThereIsContact_ShouldReturnClinicWithStatusCode200OK()
        {
            // Arrange
            var request = _fixture.Create<Guid>();
            var response = _fixture.Create<ContactResponse>();
            A.CallTo(() => _contactService.GetById(A<Guid>._)).Returns(response);

            // Act
            var actual = await _sut.GetById(request);

            // Assert
            A.CallTo(() => _contactService.GetById(A<Guid>._)).MustHaveHappenedOnceExactly();
            var actionResult = Assert.IsType<OkObjectResult>(actual);
            Assert.IsType<SuccessResult<ContactResponse>>(actionResult.Value);
        }

        [Fact]
        [Trait("HttpVerb", "GET")]
        public async Task GetById_WhenThereAreNoContactFound_ShouldReturnStatusCode404NotFound()
        {
            // Arrange
            var request = _fixture.Create<Guid>();
            var response = new ContactResponse();
            A.CallTo(() => _contactService.GetById(A<Guid>._)).Returns(response);

            // Act
            var actual = await _sut.GetById(request) as StatusCodeResult;

            // Assert
            A.CallTo(() => _contactService.GetById(A<Guid>._)).MustHaveHappenedOnceExactly();
            Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
        }

        [Fact]
        [Trait("HttpVerb", "GET")]
        public async Task GetById_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
        {
            // Arrange
            var id = _fixture.Create<Guid>();
            A.CallTo(() => _contactService.GetById(A<Guid>._)).Throws<Exception>();

            // Act
            var actual = await _sut.GetById(id) as StatusCodeResult;

            // Assert
            A.CallTo(() => _contactService.GetById(A<Guid>._)).MustHaveHappenedOnceExactly();
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
            var request = _fixture.Create<CreateContactRequest>();
            var response = _fixture.Create<ContactResponse>();
            A.CallTo(() => _contactService.Create(A<CreateContactRequest>._)).Returns(response);

            // Act
            var actual = await _sut.Create(request);

            // Assert
            A.CallTo(() => _contactService.Create(A<CreateContactRequest>._)).MustHaveHappenedOnceExactly();
            var actionResult = Assert.IsType<CreatedAtActionResult>(actual);
            Assert.IsType<SuccessResult<ContactResponse>>(actionResult.Value);
        }

        [Fact]
        [Trait("HttpVerb", "POST")]
        public async Task Create_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
        {
            // Arrange
            var req = _fixture.Create<CreateContactRequest>();
            A.CallTo(() => _contactService.Create(A<CreateContactRequest>._)).Throws<Exception>();

            // Act
            var actual = await _sut.Create(req) as StatusCodeResult;

            // Assert
            A.CallTo(() => _contactService.Create(A<CreateContactRequest>._)).MustHaveHappenedOnceExactly();
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
            A.CallTo(() => _contactService.Delete(A<Guid>._)).Returns(Task.CompletedTask);

            // Act
            var actual = await _sut.Delete(id);

            // Assert
            A.CallTo(() => _contactService.Delete(A<Guid>._)).MustHaveHappenedOnceExactly();
            var actionResult = Assert.IsType<NoContentResult>(actual);
        }

        [Fact]
        [Trait("HttpVerb", "DELETE")]
        public async Task Delete_WhenThereIsUnhandledException_ShouldReturnStatusCode500InternalServerErrorAndLogAnException()
        {
            // Arrange
            var id = _fixture.Create<Guid>();
            A.CallTo(() => _contactService.Delete(A<Guid>._)).Throws<Exception>();

            // Act
            var actual = await _sut.Delete(id) as StatusCodeResult;

            // Assert
            A.CallTo(() => _contactService.Delete(A<Guid>._)).MustHaveHappenedOnceExactly();
            A.CallTo(_logger).Where(
                    call => call.Method.Name == "Log"
                    && call.GetArgument<LogLevel>(0) == LogLevel.Error)
                .MustHaveHappened(1, Times.Exactly);
            Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
        }
    }
}
