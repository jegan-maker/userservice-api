using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Usecases.CreateUser;
using UserService.Domain.Users;
using UserService.Persistence.Repositories;

namespace UserService.Applications.Unit.Tests.Usecases.CreateUser;

public class CreateUserUseCaseTests
{
    [Fact]
    public async Task Handle_Should_Return_Success_When_Valid()
    {
        var mockRepo = new Mock<IUserRepository>();
        var handler = new CreateUserUseCase(mockRepo.Object);
        var request = new CreateUserRequest("John", "john@example.com");

        var result = await handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBe(Guid.Empty);
        mockRepo.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_On_Exception()
    {
        var mockRepo = new Mock<IUserRepository>();
        mockRepo.Setup(r => r.AddAsync(It.IsAny<User>())).ThrowsAsync(new Exception("fail"));
        var handler = new CreateUserUseCase(mockRepo.Object);
        var request = new CreateUserRequest("Error", "error@example.com");

        var result = await handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("fail");
    }
}
