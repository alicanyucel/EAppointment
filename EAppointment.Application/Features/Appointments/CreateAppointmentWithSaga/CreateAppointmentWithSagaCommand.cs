using EAppointment.Application.Saga;
using EAppointment.Application.Saga.Steps;
using EAppointment.Domain.Repositories;
using MediatR;

namespace EAppointment.Application.Features.Appointments.CreateAppointmentWithSaga;

public sealed record CreateAppointmentWithSagaCommand(
    Guid DoctorId,
    Guid PatientId,
    DateTime StartDate,
    DateTime EndDate) : IRequest<CreateAppointmentWithSagaCommandResponse>;

public sealed record CreateAppointmentWithSagaCommandResponse(string Message);

public sealed class CreateAppointmentWithSagaCommandHandler
    : IRequestHandler<CreateAppointmentWithSagaCommand, CreateAppointmentWithSagaCommandResponse>
{
    private readonly IAppointmentsRepository _appointmentRepository;

    public CreateAppointmentWithSagaCommandHandler(IAppointmentsRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<CreateAppointmentWithSagaCommandResponse> Handle(
        CreateAppointmentWithSagaCommand request,
        CancellationToken cancellationToken)
    {
        var sagaData = new CreateAppointmentSagaData
        {
            DoctorId = request.DoctorId,
            PatientId = request.PatientId,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };

        var orchestrator = new SagaOrchestrator<CreateAppointmentSagaData>();
        
        // Add steps to the saga
        orchestrator.AddStep(new ValidateDoctorAvailabilityStep(_appointmentRepository));
        orchestrator.AddStep(new CreateAppointmentStep(_appointmentRepository));
        orchestrator.AddStep(new SendNotificationStep());

        var success = await orchestrator.ExecuteAsync(sagaData, cancellationToken);

        if (!success)
        {
            return new CreateAppointmentWithSagaCommandResponse(
                "Randevu oluşturulurken bir hata oluştu ve işlem geri alındı");
        }

        return new CreateAppointmentWithSagaCommandResponse(
            "Randevu başarıyla oluşturuldu ve bildirim gönderildi");
    }
}
