using MediatR;
using Microsoft.AspNetCore.Mvc;
using AMR.Forms.Fabrica.Application.Features.Veiculos.Queries;
using AMR.Forms.Fabrica.Application.Features.Veiculos.Commands;

namespace AMR.Forms.Fabrica.API.Controllers;

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

    [HttpPost]
    public async Task<IActionResult> Cadastrar([FromBody] CadastrarVeiculoCommand command, CancellationToken ct)
        => Ok(await mediator.Send(command, ct));

    [HttpPut("{placa}")]
    public async Task<IActionResult> Editar(string placa, [FromBody] EditarVeiculoCommand command, CancellationToken ct)
        => Ok(await mediator.Send(command with { Placa = placa }, ct));
}
