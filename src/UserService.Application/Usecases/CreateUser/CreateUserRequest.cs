using MediatR;
using UserService.Application.Common.Models;


namespace UserService.Application.Usecases.CreateUser;

public sealed record CreateUserRequest(string FullName, string Email) : IRequest<Result<Guid>>;

