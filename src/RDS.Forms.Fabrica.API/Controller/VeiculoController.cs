using MediatR;
using Microsoft.AspNetCore.Mvc;
using RDS.Forms.Fabrica.Application.Features.Veiculos.Queries;

namespace RDS.Forms.Fabrica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VeiculoController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetVeiculosQuery(), ct));

    [HttpGet("filial/{cdFilial:int}")]
    public async Task<IActionResult> GetPorFilial(int cdFilial, CancellationToken ct)
        => Ok(await mediator.Send(new GetVeiculosPorFilialQuery(cdFilial), ct));
}
