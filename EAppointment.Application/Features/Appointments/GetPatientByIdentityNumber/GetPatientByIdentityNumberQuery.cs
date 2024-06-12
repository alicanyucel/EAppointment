using EAppointment.Domain.Entities;
using MediatR;
using TS.Result;

namespace EAppointment.Application.Features.Appointments.GetPatientByIdentityNumber;

public sealed record GetPatientByIdentityNumberQuery(
 string IdentityNumber) : IRequest<Result<Patient>>;
