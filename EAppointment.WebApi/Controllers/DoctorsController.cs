using EAppointment.Application.Features.Doctors.CreateDoctor;
using EAppointment.Application.Features.Doctors.GetAllDoctor;
using EAppointment.WebApi.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TS.Result;

namespace EAppointment.WebApi.Controllers;


public class DoctorsController : ApiController
{
    public DoctorsController(IMediator mediator) : base(mediator)
    {
    }
    [HttpPost]
    public async Task<IActionResult> GetAll(GetAllDoctorQuery request,CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request,cancellationToken);
        return StatusCode(response.StatusCode, response);

    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateDoctorCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);

    }
}
