using MediatR;
using UserService.Application.Common.Models;
using UserService.Domain.Users;
using UserService.Persistence.Repositories;

namespace UserService.Application.Usecases.GetUser;

public class GetUserUseCase : IRequestHandler<GetUserByIdQuery, Result<User>>
{
    private readonly IUserRepository _repository;

    public GetUserUseCase(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.Id);
        if (user == null)
            return Result<User>.Failure("User not found");

        return Result<User>.Success(user);
    }
}