using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.API.Controllers.Usecases.CreateUser;
using UserService.Application.Common.Models;
using UserService.Application.Usecases.CreateUser;

namespace UserService.API.Unit.Tests.Controllers.UseCases.CreateUser;

public class UsersControllerTests
{
    private readonly Mock<IMediator> _mediator = new();
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _controller = new UsersController(_mediator.Object);
    }

    [Fact]
    public async Task Create_Should_Return_Ok_When_Successful()
    {
        var request = new CreateUserRequest("Test User", "test@example.com");
        _mediator.Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(Result<Guid>.Success(Guid.NewGuid()));

        var result = await _controller.Create(request);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Create_Should_Return_BadRequest_When_Failed()
    {
        var request = new CreateUserRequest("Bad User", "fail@example.com");
        _mediator.Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(Result<Guid>.Failure("Creation failed"));

        var result = await _controller.Create(request);

        result.Should().BeOfType<BadRequestObjectResult>();
    }
}