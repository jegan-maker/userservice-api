using MediatR;
using UserService.Application.Common.Models;


namespace UserService.Application.Usecases.UpdateUser;

public sealed record UpdateUserRequest(Guid Id, string FullName, string Email, byte[] RowVersion) : IRequest<Result<bool>>;
