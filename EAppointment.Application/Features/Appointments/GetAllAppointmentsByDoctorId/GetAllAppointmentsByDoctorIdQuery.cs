using MediatR;
using TS.Result;

namespace EAppointment.Application.Features.Appointments.GetAllAppointmentsByDoctorId;

public sealed record GetAllAppointmentsByDoctorIdQuery(
    Guid DoctorId) : IRequest<Result<List<GetAllAppointmentsByDoctorIdQueryResponse>>>;