using EAppointment.Application.Features.Doctors.GetAllDoctor;
using EAppointment.Domain.Entities;
using EAppointment.WebApi.Abstractions;
using EAppointment.WebApi.HATEOAS;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace EAppointment.WebApi.Controllers;

[EnableRateLimiting("fixed")]
public sealed class DoctorsV2Controller : ApiController
{
    public DoctorsV2Controller(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [EnableRateLimiting("sliding")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllDoctorQuery();
        var result = await _mediator.Send(query, cancellationToken);

        var baseUrl = HttpContext.GetBaseUrl();
        var doctorList = result as List<Doctor> ?? result.ToList();
        var resources = doctorList.Select(d => d.ToHATEOAS(baseUrl)).ToList();

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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        // Implementation would go here
        return Ok();
    }
}
