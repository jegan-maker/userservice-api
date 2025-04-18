using Azure.Core;
using MediatR;
using UserService.Application.Common.Models;
using UserService.Domain.Users;
using UserService.Persistence.Repositories;

namespace UserService.Application.Usecases.CreateUser;

public class CreateUserUseCase : IRequestHandler<CreateUserRequest, Result<Guid>>
{
    private readonly IUserRepository _repository;


    public CreateUserUseCase(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Guid>> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var user = new User(Guid.NewGuid(), request.FullName, request.Email, "system");

        try
        {
            await _repository.AddAsync(user);
            return Result<Guid>.Success(user.Id);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Failure($"User creation failed: {ex.Message}");
        }
    }
}


