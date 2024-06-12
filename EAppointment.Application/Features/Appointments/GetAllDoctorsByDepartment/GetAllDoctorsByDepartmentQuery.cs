using EAppointment.Domain.Entities;
using MediatR;
using TS.Result;

namespace EAppointment.Application.Features.Appointments.GetAllDoctorsByDepartment;
public sealed record GetAllDoctorsByDepartmentQuery(
    int DepartmentValue) : IRequest<Result<List<Doctor>>>;