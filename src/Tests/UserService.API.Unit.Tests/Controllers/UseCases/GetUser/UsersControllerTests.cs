using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UserService.API.Controllers.Usecases.GetUser;
using UserService.Application.Common.Models;
using UserService.Application.Usecases.GetUser;
using UserService.Domain.Users;

namespace UserService.API.Unit.Tests.Controllers.UseCases.GetUser;

public class UsersControllerTests
{
    private readonly Mock<IMediator> _mediator = new();
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _controller = new UsersController(_mediator.Object);
    }

    [Fact]
    public async Task GetById_Should_Return_Ok_When_Found()
    {
        var user = new User (Guid.NewGuid(), "John", "john@test.com", "System");
        _mediator.Setup(m => m.Send(It.Is<GetUserByIdQuery>(q => q.Id == user.Id), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(Result<User>.Success(user));

        var result = await _controller.GetById(user.Id);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetById_Should_Return_NotFound_When_Not_Found()
    {
        _mediator.Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(Result<User>.Failure("User not found"));

        var result = await _controller.GetById(Guid.NewGuid());

        result.Should().BeOfType<NotFoundObjectResult>();
    }
}