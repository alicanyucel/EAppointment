using MediatR;
using TS.Result;

namespace EAppointment.Application.Features.Appointments.DeleteAppointmentById;

public sealed record DeleteAppointmentByIdCommand(
Guid Id) : IRequest<Result<string>>;
