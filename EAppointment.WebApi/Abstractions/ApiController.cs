using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EAppointment.WebApi.Abstractions
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiController:ControllerBase
    {
        public readonly IMediator _mediator;
        protected ApiController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
