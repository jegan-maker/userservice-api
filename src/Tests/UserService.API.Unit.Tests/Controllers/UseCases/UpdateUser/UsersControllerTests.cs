using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UserService.API.Controllers.Usecases.UpdateUser;
using UserService.Application.Common.Models;
using UserService.Application.Usecases.UpdateUser;

namespace UserService.API.Unit.Tests.Controllers.UseCases.UpdateUser;

public class UsersControllerTests
{
    private readonly Mock<IMediator> _mediator = new();
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _controller = new UsersController(_mediator.Object);
    }

    [Fact]
    public async Task Update_Should_Return_NoContent_When_Successful()
    {
        var request = new UpdateUserRequest(Guid.NewGuid(), "Updated", "update@test.com", new byte[0]);
        _mediator.Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(Result<bool>.Success(true));

        var result = await _controller.Update(request);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Update_Should_Return_BadRequest_On_Failure()
    {
        var request = new UpdateUserRequest(Guid.NewGuid(), "Update", "fail@test.com", new byte[0]);
        _mediator.Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(Result<bool>.Failure("Failed to update"));

        var result = await _controller.Update(request);

        result.Should().BeOfType<BadRequestObjectResult>();
    }
}
