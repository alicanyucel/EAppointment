using EAppointment.Application.Features.Doctors.CreateDoctor;
using EAppointment.Domain.Entities;
using EAppointment.Domain.Enums;
using EAppointment.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace EAppointment.Tests.Application.Features.Doctors;

public class CreateDoctorCommandHandlerTests
{
    private readonly Mock<IDoctorRepository> _mockDoctorRepository;
    private readonly CreateDoctorComamndHandler _handler;

    public CreateDoctorCommandHandlerTests()
    {
        _mockDoctorRepository = new Mock<IDoctorRepository>();
        _handler = new CreateDoctorComamndHandler(_mockDoctorRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateDoctor_WhenValidCommandProvided()
    {
        // Arrange
        var command = new CreateDoctorCommand(
            "Ali",
            "Yılmaz",
            DepartmentEnum.Cardiology
        );

        _mockDoctorRepository
            .Setup(x => x.CreateAsync(It.IsAny<Doctor>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Message.Should().Be("Doktor kaydı başarıyla tamamlandı");
        
        _mockDoctorRepository.Verify(
            x => x.CreateAsync(It.Is<Doctor>(d => 
                d.FirstName == "Ali" && 
                d.LastName == "Yılmaz" && 
                d.Department == DepartmentEnum.Cardiology), 
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Theory]
    [InlineData("", "Yılmaz", DepartmentEnum.Cardiology)]
    [InlineData("Ali", "", DepartmentEnum.Cardiology)]
    public async Task Handle_ShouldThrowException_WhenInvalidDataProvided(
        string firstName, 
        string lastName, 
        DepartmentEnum department)
    {
        // Arrange
        var command = new CreateDoctorCommand(firstName, lastName, department);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
}
