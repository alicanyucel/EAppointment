using EAppointment.Application.Features.Auth.Login;
using EAppointment.WebApi.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

namespace EAppointment.WebApi.Controllers
{

    public sealed class AuthController : ApiController
    {
        public AuthController(IMediator mediator) : base(mediator)
        {

        }

        [HttpPost]
        public async Task<IActionResult>Login(LoginCommand request, CancellationToken CancellationToken)
        {

            var response =await  _mediator.Send(request, CancellationToken);
            return StatusCode(response.StatusCode, response);
        }
    }
}