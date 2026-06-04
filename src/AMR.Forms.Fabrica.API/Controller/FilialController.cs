using MediatR;
using Microsoft.AspNetCore.Mvc;
using AMR.Forms.Fabrica.Application.Features.Filiais.Queries;

namespace AMR.Forms.Fabrica.API.Controller;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
public class FilialController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetFiliaisQuery(), ct));
}
