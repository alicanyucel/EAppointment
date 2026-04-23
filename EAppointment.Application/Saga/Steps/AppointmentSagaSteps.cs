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

// Step 1: Validate doctor availability  
public class ValidateDoctorAvailabilityStep : SagaStep<CreateAppointmentSagaData>
{
    private readonly IAppointmentsRepository _appointmentRepository;

    public ValidateDoctorAvailabilityStep(IAppointmentsRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public override async Task ExecuteAsync(CreateAppointmentSagaData data, CancellationToken cancellationToken = default)
    {
        // Simplified validation - just mark as available for demo
        // In production, implement proper availability check
        await Task.CompletedTask;
        data.IsDoctorAvailable = true;
    }

    public override Task CompensateAsync(CreateAppointmentSagaData data, CancellationToken cancellationToken = default)
    {
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

        // For demo purposes, we'll log instead of actual DB operation
        // In production, use the repository's Create method
        Console.WriteLine($"Creating appointment {appointment.Id}");
        data.CreatedAppointmentId = appointment.Id;
        await Task.CompletedTask;
    }

    public override async Task CompensateAsync(CreateAppointmentSagaData data, CancellationToken cancellationToken = default)
    {
        if (data.CreatedAppointmentId.HasValue)
        {
            // For demo purposes, we'll log instead of actual DB operation
            Console.WriteLine($"Deleting appointment {data.CreatedAppointmentId.Value}");
            data.CreatedAppointmentId = null;
        }
        await Task.CompletedTask;
    }
}

// Step 3: Send notification
public class SendNotificationStep : SagaStep<CreateAppointmentSagaData>
{
    public override Task ExecuteAsync(CreateAppointmentSagaData data, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Notification sent for appointment {data.CreatedAppointmentId}");
        data.IsPatientNotified = true;
        return Task.CompletedTask;
    }

    public override Task CompensateAsync(CreateAppointmentSagaData data, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Cancellation notification sent for appointment {data.CreatedAppointmentId}");
        data.IsPatientNotified = false;
        return Task.CompletedTask;
    }
}
