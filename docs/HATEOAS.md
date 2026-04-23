# HATEOAS Implementation Guide

## Overview

EAppointment API implements HATEOAS (Hypermedia as the Engine of Application State) to make the API self-descriptive and discoverable.

## What is HATEOAS?

HATEOAS is a constraint of REST that allows clients to dynamically navigate the API through hypermedia links provided in responses.

## Benefits

- **Self-Documenting**: API responses include available actions
- **Reduced Coupling**: Clients don't hardcode URLs
- **Discoverability**: Easy to explore API capabilities
- **Versioning**: Changes don't break clients

## Response Structure

### Doctor Resource
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "firstName": "John",
  "lastName": "Doe",
  "department": 1,
  "links": [
    {
      "href": "http://localhost:5000/api/doctors/3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "rel": "self",
      "method": "GET"
    },
    {
      "href": "http://localhost:5000/api/doctors/3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "rel": "update",
      "method": "PUT"
    },
    {
      "href": "http://localhost:5000/api/doctors/3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "rel": "delete",
      "method": "DELETE"
    },
    {
      "href": "http://localhost:5000/api/appointments/getallbydoctorid/3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "rel": "appointments",
      "method": "GET"
    },
    {
      "href": "http://localhost:5000/api/doctors",
      "rel": "all-doctors",
      "method": "GET"
    }
  ]
}
```

### Collection Response
```json
{
  "data": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "firstName": "John",
      "lastName": "Doe",
      "department": 1,
      "links": [...]
    }
  ],
  "_links": {
    "self": {
      "href": "http://localhost:5000/api/v2/doctors",
      "method": "GET"
    },
    "create": {
      "href": "http://localhost:5000/api/v2/doctors",
      "method": "POST"
    }
  }
}
```

## Link Relations

### Standard Relations (RFC 5988)

- **self**: Current resource
- **create**: Create new resource
- **update**: Update resource
- **delete**: Delete resource
- **next**: Next page
- **prev**: Previous page
- **first**: First page
- **last**: Last page

### Custom Relations

- **appointments**: Related appointments
- **doctor**: Related doctor
- **patient**: Related patient
- **complete**: Complete appointment

## Implementation

### Enable HATEOAS for an endpoint

```csharp
[HttpGet]
public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
{
    var query = new GetAllDoctorQuery();
    var result = await _mediator.Send(query, cancellationToken);
    
    var baseUrl = HttpContext.GetBaseUrl();
    var resources = result.Select(d => d.ToHATEOAS(baseUrl)).ToList();
    
    return Ok(new
    {
        data = resources,
        _links = new
        {
            self = new { href = $"{baseUrl}/api/v2/doctors", method = "GET" },
            create = new { href = $"{baseUrl}/api/v2/doctors", method = "POST" }
        }
    });
}
```

### Extension Methods

```csharp
public static DoctorResourceDto ToHATEOAS(this Doctor doctor, string baseUrl)
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
    
    return resource;
}
```

## Pagination with HATEOAS

```json
{
  "data": [...],
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 5,
  "totalRecords": 50,
  "_links": {
    "self": {
      "href": "http://localhost:5000/api/doctors?page=1&size=10",
      "method": "GET"
    },
    "first": {
      "href": "http://localhost:5000/api/doctors?page=1&size=10",
      "method": "GET"
    },
    "last": {
      "href": "http://localhost:5000/api/doctors?page=5&size=10",
      "method": "GET"
    },
    "next": {
      "href": "http://localhost:5000/api/doctors?page=2&size=10",
      "method": "GET"
    }
  }
}
```

## Conditional Links

Links can be conditionally included based on resource state:

```csharp
public static AppointmentResourceDto ToHATEOAS(this Appointment appointment, string baseUrl)
{
    var resource = new AppointmentResourceDto { ... };

    resource.AddLink($"{baseUrl}/api/appointments/{appointment.Id}", "self", "GET");
    
    if (!appointment.IsCompleted)
    {
        resource.AddLink($"{baseUrl}/api/appointments/{appointment.Id}/complete", "complete", "POST");
        resource.AddLink($"{baseUrl}/api/appointments/{appointment.Id}", "update", "PUT");
        resource.AddLink($"{baseUrl}/api/appointments/{appointment.Id}", "delete", "DELETE");
    }
    else
    {
        resource.AddLink($"{baseUrl}/api/appointments/{appointment.Id}/receipt", "receipt", "GET");
    }
    
    return resource;
}
```

## Versioning with HATEOAS

### v1 (No HATEOAS)
```
GET /api/doctors
```

### v2 (With HATEOAS)
```
GET /api/v2/doctors
```

## Client Usage

### JavaScript Example

```javascript
async function navigateAPI() {
  // Start at root
  const response = await fetch('http://localhost:5000/api/v2/doctors');
  const data = await response.json();
  
  // Follow links
  if (data._links.create) {
    await fetch(data._links.create.href, {
      method: data._links.create.method,
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({...})
    });
  }
  
  // Navigate to related resources
  const firstDoctor = data.data[0];
  const appointmentsLink = firstDoctor.links.find(l => l.rel === 'appointments');
  
  if (appointmentsLink) {
    const appointments = await fetch(appointmentsLink.href);
    // Process appointments
  }
}
```

### C# Example

```csharp
public class HATEOASClient
{
    private readonly HttpClient _httpClient;

    public async Task<T> FollowLink<T>(Link link)
    {
        var response = await _httpClient.SendAsync(new HttpRequestMessage
        {
            Method = new HttpMethod(link.Method),
            RequestUri = new Uri(link.Href)
        });
        
        return await response.Content.ReadFromJsonAsync<T>();
    }
}
```

## Best Practices

1. **Consistent Link Structure**
   - Always include `href`, `rel`, and `method`
   - Use standard rel values when possible

2. **Absolute URLs**
   - Always use absolute URLs
   - Include scheme and host

3. **State-Based Links**
   - Only include relevant links based on resource state
   - Hide unavailable actions

4. **Documentation**
   - Document link relations
   - Explain state transitions

5. **Caching**
   - Consider cache headers for link-heavy responses
   - Use ETags for resource validation

## Testing HATEOAS

```csharp
[Fact]
public async Task GetDoctor_ShouldIncludeHATEOASLinks()
{
    // Arrange
    var client = _factory.CreateClient();
    
    // Act
    var response = await client.GetAsync("/api/v2/doctors/123");
    var content = await response.Content.ReadFromJsonAsync<DoctorResourceDto>();
    
    // Assert
    content.Links.Should().NotBeEmpty();
    content.Links.Should().Contain(l => l.Rel == "self");
    content.Links.Should().Contain(l => l.Rel == "update");
}
```

## Maturity Levels

### Level 0: No HATEOAS
```json
{
  "id": "123",
  "name": "John Doe"
}
```

### Level 1: Basic Links
```json
{
  "id": "123",
  "name": "John Doe",
  "links": [
    { "href": "/api/doctors/123", "rel": "self" }
  ]
}
```

### Level 2: Actions & Relations
```json
{
  "id": "123",
  "name": "John Doe",
  "links": [
    { "href": "/api/doctors/123", "rel": "self", "method": "GET" },
    { "href": "/api/doctors/123", "rel": "update", "method": "PUT" },
    { "href": "/api/doctors/123/appointments", "rel": "appointments", "method": "GET" }
  ]
}
```

### Level 3: Rich Hypermedia (HATEOAS Full Implementation)
Includes forms, templates, and complete state machine representation.

## References

- [Richardson Maturity Model](https://martinfowler.com/articles/richardsonMaturityModel.html)
- [RFC 5988 - Web Linking](https://tools.ietf.org/html/rfc5988)
- [HATEOAS Pattern](https://restfulapi.net/hateoas/)
