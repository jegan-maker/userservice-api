using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using UserService.Application.Usecases.UpdateUser;
using UserService.Domain.Users;
using UserService.Persistence.Repositories;

namespace UserService.Applications.Unit.Tests.Usecases.UpdateUser;

public class UpdateUserUseCaseTests
{
    [Fact]
    public async Task Handle_Should_Return_Success_When_Valid()
    {
        var user = new User(Guid.NewGuid(), "Old", "old@example.com", "System");  

        var mockRepo = new Mock<IUserRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
        var handler = new UpdateUserUseCase(mockRepo.Object);
        var request = new UpdateUserRequest(user.Id, "New", "new@example.com", new byte[0]);

        var result = await handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        mockRepo.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_When_User_Not_Found()
    {
        var mockRepo = new Mock<IUserRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);
        var handler = new UpdateUserUseCase(mockRepo.Object);
        var request = new UpdateUserRequest(Guid.NewGuid(), "Test", "test@test.com", new byte[0]);

        var result = await handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("User not found.");
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_On_Exception()
    {
        var user = new User (Guid.NewGuid(), "", "", "System");
        var mockRepo = new Mock<IUserRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
        mockRepo.Setup(r => r.UpdateAsync(It.IsAny<User>())).ThrowsAsync(new Exception("update failed"));

        var handler = new UpdateUserUseCase(mockRepo.Object);
        var request = new UpdateUserRequest(user.Id, "Name", "email@test.com", new byte[0]);

        var result = await handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("update failed");
    }

    [Fact]
    public async Task Handler_Should_Return_Failure_When_ConcurrencyException_Thrown()
    {
        var user = new User(Guid.NewGuid(), "Concurrent", "conflict@example.com", "System");

        var mockRepo = new Mock<IUserRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
        mockRepo.Setup(r => r.UpdateAsync((It.IsAny<User>()))).ThrowsAsync(new DbUpdateConcurrencyException());

        var handler = new UpdateUserUseCase(mockRepo.Object);
        var command = new UpdateUserRequest(user.Id, "New Name", "new@email.com", user.RowVersion);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("The record was modified by another process");
    }
}