using EAppointment.Application.Features.Appointments.CreateAppointment;
using EAppointment.Domain.Entities;
using EAppointment.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace EAppointment.Tests.Application.Features.Appointments;

public class CreateAppointmentCommandHandlerTests
{
    private readonly Mock<IAppointmentsRepository> _mockAppointmentRepository;
    private readonly CreateAppointmentCommandHandler _handler;

    public CreateAppointmentCommandHandlerTests()
    {
        _mockAppointmentRepository = new Mock<IAppointmentsRepository>();
        _handler = new CreateAppointmentCommandHandler(_mockAppointmentRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateAppointment_WhenValidCommandProvided()
    {
        // Arrange
        var startDate = DateTime.Now.AddDays(1);
        var endDate = startDate.AddHours(1);
        
        var command = new CreateAppointmentCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            startDate,
            endDate
        );

        _mockAppointmentRepository
            .Setup(x => x.IsAppointmentDateNotAvailable(
                It.IsAny<Guid>(), 
                It.IsAny<DateTime>(), 
                It.IsAny<DateTime>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _mockAppointmentRepository
            .Setup(x => x.CreateAsync(It.IsAny<Appointment>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Message.Should().Be("Randevu kaydı başarıyla tamamlandı");
        
        _mockAppointmentRepository.Verify(
            x => x.CreateAsync(It.IsAny<Appointment>(), It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenAppointmentDateNotAvailable()
    {
        // Arrange
        var startDate = DateTime.Now.AddDays(1);
        var endDate = startDate.AddHours(1);
        
        var command = new CreateAppointmentCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            startDate,
            endDate
        );

        _mockAppointmentRepository
            .Setup(x => x.IsAppointmentDateNotAvailable(
                It.IsAny<Guid>(), 
                It.IsAny<DateTime>(), 
                It.IsAny<DateTime>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
}
