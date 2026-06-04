using MediatR;
using Microsoft.AspNetCore.Mvc;
using AMR.Forms.Fabrica.Application.Features.Fichas.Commands;
using AMR.Forms.Fabrica.Application.Features.Fichas.Queries;

namespace AMR.Forms.Fabrica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
public class FichaController(IMediator mediator) : ControllerBase
{
    // GET api/Ficha?cdFilial=1
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int cdFilial = 1,
        [FromQuery] DateOnly? dtInicio = null,
        [FromQuery] DateOnly? dtFim = null,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetFichasQuery(cdFilial, dtInicio, dtFim), ct);
        return Ok(result);
    }

    // GET api/Ficha/42
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetFichaByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    // POST api/Ficha
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] AbrirFichaCommand command,
        CancellationToken ct = default)
    {
        var id = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    // PATCH api/Ficha/42/passo
    [HttpPatch("{id:int}/passo")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AvancarPasso(
        int id,
        [FromBody] AvancarPassoRequest request,
        CancellationToken ct = default)
    {
        await mediator.Send(new AvancarPassoCommand(id, request.ProximoPasso), ct);
        return NoContent();
    }

    // PATCH api/Ficha/42/saida
    [HttpPatch("{id:int}/saida")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RegistrarSaida(int id, CancellationToken ct = default)
    {
        await mediator.Send(new RegistrarSaidaCommand(id), ct);
        return NoContent();
    }
}

public record AvancarPassoRequest(int ProximoPasso);
