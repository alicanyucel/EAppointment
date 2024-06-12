using EAppointment.Domain.Entities;
using EAppointment.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace EAppointment.Application.Features.Appointments.GetAllAppointmentsByDoctorId;
internal sealed class GetAllAppointmentsByDoctorIdQueryHandler(
    IAppointmentsRepository appointmentRepository) : IRequestHandler<GetAllAppointmentsByDoctorIdQuery, Result<List<GetAllAppointmentsByDoctorIdQueryResponse>>>
{
    public async Task<Result<List<GetAllAppointmentsByDoctorIdQueryResponse>>> Handle(GetAllAppointmentsByDoctorIdQuery request, CancellationToken cancellationToken)
    {
        List<Appointment> appointments =
            await appointmentRepository
            .Where(p => p.DoctorId == request.DoctorId)
            .Include(p => p.Patient)
            .ToListAsync(cancellationToken);


        List<GetAllAppointmentsByDoctorIdQueryResponse> response =
            appointments.Select(s =>
            new GetAllAppointmentsByDoctorIdQueryResponse(
                s.Id,
                s.StartDate,
                s.EndDate,
                s.Patient!.FullName,
                s.Patient))
            .ToList();

        return response;
    }
}