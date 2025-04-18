using FluentAssertions;
using Moq;
using UserService.Application.Usecases.GetUser;
using UserService.Domain.Users;
using UserService.Persistence.Repositories;

namespace UserService.Applications.Unit.Tests.Usecases.GetUser;

public class GetUserUseCaseTests
{
    public class GetUserByIdHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Return_User_When_Found()
        {

            var user = new User(Guid.NewGuid(), "Found", "found@example.com", "System");
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

            var handler = new GetUserUseCase(mockRepo.Object);
            var result = await handler.Handle(new GetUserByIdQuery(user.Id), CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value!.Email.Should().Be("found@example.com");
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_NotFound()
        {
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);

            var handler = new GetUserUseCase(mockRepo.Object);
            var result = await handler.Handle(new GetUserByIdQuery(Guid.NewGuid()), CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("User not found");
        }
    }
}
