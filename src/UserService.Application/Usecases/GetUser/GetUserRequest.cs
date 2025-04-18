using MediatR;
using UserService.Application.Common.Models;
using UserService.Domain.Users;

namespace UserService.Application.Usecases.GetUser;

public sealed record GetUserByIdQuery(Guid Id) : IRequest<Result<User>>;
