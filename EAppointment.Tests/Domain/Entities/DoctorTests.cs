using EAppointment.Domain.Entities;
using EAppointment.Domain.Enums;
using FluentAssertions;

namespace EAppointment.Tests.Domain.Entities;

public class DoctorTests
{
    [Fact]
    public void Doctor_ShouldBeCreated_WithValidProperties()
    {
        // Arrange & Act
        var doctor = new Doctor
        {
            Id = Guid.NewGuid(),
            FirstName = "Ahmet",
            LastName = "Kaya",
            Department = DepartmentEnum.Cardiology
        };

        // Assert
        doctor.Should().NotBeNull();
        doctor.Id.Should().NotBeEmpty();
        doctor.FirstName.Should().Be("Ahmet");
        doctor.LastName.Should().Be("Kaya");
        doctor.Department.Should().Be(DepartmentEnum.Cardiology);
    }

    [Fact]
    public void Doctor_FullName_ShouldReturnCombinedName()
    {
        // Arrange
        var doctor = new Doctor
        {
            FirstName = "Mehmet",
            LastName = "Demir",
            Department = DepartmentEnum.Neurology
        };

        // Act
        var fullName = $"{doctor.FirstName} {doctor.LastName}";

        // Assert
        fullName.Should().Be("Mehmet Demir");
    }
}
