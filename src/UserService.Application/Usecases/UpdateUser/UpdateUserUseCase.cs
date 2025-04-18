using Azure.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Common.Models;
using UserService.Domain.Users;
using UserService.Persistence.Repositories;

namespace UserService.Application.Usecases.UpdateUser;

public class UpdateUserUseCase : IRequestHandler<UpdateUserRequest, Result<bool>>
{
    private readonly IUserRepository _repository;

    public UpdateUserUseCase(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.Id);
        if (user == null)
            return Result<bool>.Failure("User not found.");

        user.Update(request.FullName, request.Email, "system", request.RowVersion);

        try
        {
            await _repository.UpdateAsync(user);
            return Result<bool>.Success(true);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Result<bool>.Failure("The record was modified by another process. Please reload and try again.");
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Update failed: {ex.Message}");
        }
    }
}
