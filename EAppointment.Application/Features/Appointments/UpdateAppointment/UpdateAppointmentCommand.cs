using MediatR;
using TS.Result;

namespace EAppointment.Application.Features.Appointments.UpdateAppointment;

public sealed record UpdateAppointmentCommand(
    Guid Id,
    string StartDate,
    string EndDate) : IRequest<Result<string>>;