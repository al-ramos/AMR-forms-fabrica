using MediatR;
using Microsoft.AspNetCore.Mvc;
using AMR.Forms.Fabrica.Application.Features.NotasFiscais.Queries;

namespace AMR.Forms.Fabrica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotaFiscalController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int cdFilial = 1,
        [FromQuery] DateOnly? dtInicio = null,
        [FromQuery] DateOnly? dtFim = null,
        CancellationToken ct = default)
        => Ok(await mediator.Send(new GetNotasFiscaisQuery(cdFilial, dtInicio, dtFim), ct));

    [HttpGet("{numero:int}/{serie}/itens")]
    public async Task<IActionResult> GetItens(int numero, string serie, CancellationToken ct)
    {
        var result = await mediator.Send(new GetNotaFiscalItensQuery(numero, serie), ct);
        return result is null ? NotFound() : Ok(result);
    }
}
