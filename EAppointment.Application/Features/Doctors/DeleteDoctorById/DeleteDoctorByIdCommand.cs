using MediatR;
using TS.Result;

namespace EAppointment.Application.Features.Doctors.DeleteDoctorById;

public sealed record DeleteDoctorByIdCommand(
    Guid Id) : IRequest<Result<string>>;


