using MediatR;
using Microsoft.AspNetCore.Mvc;
using RDS.Forms.Fabrica.Application.Features.Veiculos.Commands;
using RDS.Forms.Fabrica.Domain.Interfaces;

namespace RDS.Forms.Fabrica.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VeiculoController(IVeiculoRepository repo, IMediator mediator) : ControllerBase
{
    // GET api/veiculo
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var veiculos = await repo.ListarTodasAsync();
        return Ok(veiculos.Select(v => new
        {
            v.Placa,
            v.CodigoFilial,
            v.UfVeiculo,
            v.CodigoRntc,
        }));
    }

    // GET api/veiculo/filial/1
    [HttpGet("filial/{codigoFilial}")]
    public async Task<IActionResult> GetByFilial(int codigoFilial)
    {
        var veiculos = await repo.ListarPorFilialAsync(codigoFilial);
        return Ok(veiculos.Select(v => new
        {
            v.Placa,
            v.CodigoFilial,
            v.UfVeiculo,
            v.CodigoRntc,
        }));
    }

    // GET api/veiculo/ABC-1234
    [HttpGet("{placa}")]
    public async Task<IActionResult> GetByPlaca(string placa)
    {
        var veiculo = await repo.ObterPorPlacaAsync(placa);
        if (veiculo is null) return NotFound();
        return Ok(new
        {
            veiculo.Placa,
            veiculo.CodigoFilial,
            veiculo.UfVeiculo,
            veiculo.CodigoRntc,
        });
    }

    // POST api/veiculo
    [HttpPost]
    public async Task<IActionResult> Cadastrar(
        [FromBody] CadastrarVeiculoCommand command,
        CancellationToken ct)
    {
        var placa = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetAll), new { }, new { placa });
    }
}
