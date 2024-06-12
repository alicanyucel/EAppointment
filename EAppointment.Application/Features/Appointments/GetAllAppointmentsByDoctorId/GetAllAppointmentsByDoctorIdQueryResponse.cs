using EAppointment.Domain.Entities;

namespace EAppointment.Application.Features.Appointments.GetAllAppointmentsByDoctorId;
public sealed record GetAllAppointmentsByDoctorIdQueryResponse(
    Guid Id,
    DateTime StartDate,
    DateTime EndDate,
    string Title,
    Patient Patient);