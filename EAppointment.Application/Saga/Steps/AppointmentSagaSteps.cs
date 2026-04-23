using EAppointment.Domain.Entities;
using EAppointment.Domain.Repositories;

namespace EAppointment.Application.Saga.Steps;

// Data transfer object for appointment creation saga
public class CreateAppointmentSagaData
{
    public Guid DoctorId { get; set; }
    public Guid PatientId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid? CreatedAppointmentId { get; set; }
    public bool IsDoctorAvailable { get; set; }
    public bool IsPatientNotified { get; set; }
}

// Step 1: Validate doctor availability (simplified)
public class ValidateDoctorAvailabilityStep : SagaStep<CreateAppointmentSagaData>
{
    private readonly IAppointmentsRepository _appointmentRepository;

    public ValidateDoctorAvailabilityStep(IAppointmentsRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public override async Task ExecuteAsync(CreateAppointmentSagaData data, CancellationToken cancellationToken = default)
    {
        // Simplified validation - check if any appointments exist for this doctor at this time
        var existingAppointments = await _appointmentRepository.GetAll(cancellationToken);
        var hasConflict = existingAppointments.Any(a => 
            a.DoctorId == data.DoctorId && 
            a.StartDate < data.EndDate && 
            a.EndDate > data.StartDate);

        if (hasConflict)
        {
            throw new InvalidOperationException("Doktor bu tarihte müsait değil");
        }

        data.IsDoctorAvailable = true;
        await Task.CompletedTask;
    }

    public override Task CompensateAsync(CreateAppointmentSagaData data, CancellationToken cancellationToken = default)
    {
        // No compensation needed for validation
        data.IsDoctorAvailable = false;
        return Task.CompletedTask;
    }
}

// Step 2: Create appointment
public class CreateAppointmentStep : SagaStep<CreateAppointmentSagaData>
{
    private readonly IAppointmentsRepository _appointmentRepository;

    public CreateAppointmentStep(IAppointmentsRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public override async Task ExecuteAsync(CreateAppointmentSagaData data, CancellationToken cancellationToken = default)
    {
        var appointment = new Appointment
        {
            Id = Guid.NewGuid(),
            DoctorId = data.DoctorId,
            PatientId = data.PatientId,
            StartDate = data.StartDate,
            EndDate = data.EndDate,
            IsCompleted = false
        };

        await _appointmentRepository.Create(appointment, cancellationToken);
        data.CreatedAppointmentId = appointment.Id;
    }

    public override async Task CompensateAsync(CreateAppointmentSagaData data, CancellationToken cancellationToken = default)
    {
        if (data.CreatedAppointmentId.HasValue)
        {
            var appointment = await _appointmentRepository.GetById(data.CreatedAppointmentId.Value, cancellationToken);
            if (appointment != null)
            {
                await _appointmentRepository.Delete(appointment, cancellationToken);
            }
            data.CreatedAppointmentId = null;
        }
    }
}

// Step 3: Send notification (simulated)
public class SendNotificationStep : SagaStep<CreateAppointmentSagaData>
{
    // In real scenario, inject notification service
    public override Task ExecuteAsync(CreateAppointmentSagaData data, CancellationToken cancellationToken = default)
    {
        // Simulate sending notification
        Console.WriteLine($"Notification sent for appointment {data.CreatedAppointmentId}");
        data.IsPatientNotified = true;
        return Task.CompletedTask;
    }

    public override Task CompensateAsync(CreateAppointmentSagaData data, CancellationToken cancellationToken = default)
    {
        // Simulate sending cancellation notification
        Console.WriteLine($"Cancellation notification sent for appointment {data.CreatedAppointmentId}");
        data.IsPatientNotified = false;
        return Task.CompletedTask;
    }
}
