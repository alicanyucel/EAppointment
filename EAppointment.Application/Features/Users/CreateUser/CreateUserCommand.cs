using MediatR;
using TS.Result;

namespace EAppointment.Application.Features.Users.CreateUser;
public sealed record CreateUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    string Password,
    List<Guid> RoleIds) : IRequest<Result<string>>;