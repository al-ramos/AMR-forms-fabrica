using MediatR;
using Microsoft.AspNetCore.Mvc;
using RDS.Forms.Fabrica.Application.Features.Pedidos.Queries;

namespace RDS.Forms.Fabrica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidoController(IMediator mediator) : ControllerBase
{
    // GET api/Pedido?cdFilial=1
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int cdFilial = 1,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetPedidosQuery(cdFilial), ct);
        return Ok(result);
    }
}
