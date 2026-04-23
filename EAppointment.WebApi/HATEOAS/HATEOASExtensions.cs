namespace EAppointment.WebApi.HATEOAS;

public class Link
{
    public string Href { get; set; } = string.Empty;
    public string Rel { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;

    public Link(string href, string rel, string method)
    {
        Href = href;
        Rel = rel;
        Method = method;
    }
}

public class ResourceWithLinks
{
    public List<Link> Links { get; set; } = new();

    public void AddLink(string href, string rel, string method)
    {
        Links.Add(new Link(href, rel, method));
    }
}

public class DoctorResourceDto : ResourceWithLinks
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Department { get; set; }
}

public class PatientResourceDto : ResourceWithLinks
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string IdentityNumber { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Town { get; set; } = string.Empty;
    public string FullAddress { get; set; } = string.Empty;
}

public class AppointmentResourceDto : ResourceWithLinks
{
    public Guid Id { get; set; }
    public Guid DoctorId { get; set; }
    public Guid PatientId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsCompleted { get; set; }
}

public static class HATEOASExtensions
{
    public static DoctorResourceDto ToHATEOAS(this EAppointment.Domain.Entities.Doctor doctor, string baseUrl)
    {
        var resource = new DoctorResourceDto
        {
            Id = doctor.Id,
            FirstName = doctor.FirstName,
            LastName = doctor.LastName,
            Department = (int)doctor.Department
        };

        resource.AddLink($"{baseUrl}/api/doctors/{doctor.Id}", "self", "GET");
        resource.AddLink($"{baseUrl}/api/doctors/{doctor.Id}", "update", "PUT");
        resource.AddLink($"{baseUrl}/api/doctors/{doctor.Id}", "delete", "DELETE");
        resource.AddLink($"{baseUrl}/api/appointments/getallbydoctorid/{doctor.Id}", "appointments", "GET");
        resource.AddLink($"{baseUrl}/api/doctors", "all-doctors", "GET");

        return resource;
    }

    public static PatientResourceDto ToHATEOAS(this EAppointment.Domain.Entities.Patient patient, string baseUrl)
    {
        var resource = new PatientResourceDto
        {
            Id = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            IdentityNumber = patient.IdentityNumber,
            City = patient.City,
            Town = patient.Town,
            FullAddress = patient.FullAdress  // Note: Domain uses FullAdress (typo in original)
        };

        resource.AddLink($"{baseUrl}/api/patients/{patient.Id}", "self", "GET");
        resource.AddLink($"{baseUrl}/api/patients/{patient.Id}", "update", "PUT");
        resource.AddLink($"{baseUrl}/api/patients/{patient.Id}", "delete", "DELETE");
        resource.AddLink($"{baseUrl}/api/appointments/getbyidentitynumber/{patient.IdentityNumber}", "appointments", "GET");
        resource.AddLink($"{baseUrl}/api/patients", "all-patients", "GET");

        return resource;
    }

    public static AppointmentResourceDto ToHATEOAS(this EAppointment.Domain.Entities.Appointment appointment, string baseUrl)
    {
        var resource = new AppointmentResourceDto
        {
            Id = appointment.Id,
            DoctorId = appointment.DoctorId,
            PatientId = appointment.PatientId,
            StartDate = appointment.StartDate,
            EndDate = appointment.EndDate,
            IsCompleted = appointment.IsCompleted
        };

        resource.AddLink($"{baseUrl}/api/appointments/{appointment.Id}", "self", "GET");
        resource.AddLink($"{baseUrl}/api/appointments/{appointment.Id}", "update", "PUT");
        resource.AddLink($"{baseUrl}/api/appointments/{appointment.Id}", "delete", "DELETE");
        resource.AddLink($"{baseUrl}/api/doctors/{appointment.DoctorId}", "doctor", "GET");
        resource.AddLink($"{baseUrl}/api/patients/{appointment.PatientId}", "patient", "GET");

        if (!appointment.IsCompleted)
        {
            resource.AddLink($"{baseUrl}/api/appointments/{appointment.Id}/complete", "complete", "POST");
        }

        return resource;
    }

    public static string GetBaseUrl(this HttpContext httpContext)
    {
        return $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";
    }
}
