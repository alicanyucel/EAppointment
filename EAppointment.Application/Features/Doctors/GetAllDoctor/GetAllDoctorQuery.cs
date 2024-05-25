
using EAppointment.Domain.Entities;
using MediatR;
using TS.Result;

namespace EAppointment.Application.Features.Doctors.GetAllDoctor;

public sealed record   GetAllDoctorQuery():IRequest<Result<List<Doctor>>>;
