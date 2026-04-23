using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using EAppointment.Domain.Entities;
using EAppointment.Domain.Enums;

namespace EAppointment.Benchmarks;

[MemoryDiagnoser]
[RankColumn]
public class EntityCreationBenchmarks
{
    [Benchmark]
    public Doctor CreateDoctor()
    {
        return new Doctor
        {
            Id = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "Doctor",
            Department = DepartmentEnum.Cardiology
        };
    }

    [Benchmark]
    public Patient CreatePatient()
    {
        return new Patient
        {
            Id = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "Patient",
            IdentityNumber = "12345678901",
            City = "Istanbul",
            Town = "Kadikoy",
            FullAddress = "Test Address"
        };
    }

    [Benchmark]
    public Appointment CreateAppointment()
    {
        return new Appointment
        {
            Id = Guid.NewGuid(),
            DoctorId = Guid.NewGuid(),
            PatientId = Guid.NewGuid(),
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddHours(1),
            IsCompleted = false
        };
    }
}

[MemoryDiagnoser]
public class ListOperationsBenchmarks
{
    private List<Doctor> _doctors = new();

    [GlobalSetup]
    public void Setup()
    {
        _doctors = Enumerable.Range(0, 1000)
            .Select(i => new Doctor
            {
                Id = Guid.NewGuid(),
                FirstName = $"Doctor{i}",
                LastName = $"LastName{i}",
                Department = (DepartmentEnum)(i % 10)
            })
            .ToList();
    }

    [Benchmark]
    public List<Doctor> FilterByDepartment()
    {
        return _doctors.Where(d => d.Department == DepartmentEnum.Cardiology).ToList();
    }

    [Benchmark]
    public Doctor? FindById()
    {
        var targetId = _doctors[500].Id;
        return _doctors.FirstOrDefault(d => d.Id == targetId);
    }

    [Benchmark]
    public List<Doctor> SortByLastName()
    {
        return _doctors.OrderBy(d => d.LastName).ToList();
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<EntityCreationBenchmarks>();
        var listSummary = BenchmarkRunner.Run<ListOperationsBenchmarks>();
    }
}
