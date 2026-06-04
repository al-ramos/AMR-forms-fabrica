using MediatR;
using Microsoft.AspNetCore.Mvc;
using AMR.Forms.Fabrica.Application.Features.NotasFiscais.Queries;
using AMR.Forms.Fabrica.Application.Features.NotasFiscais.Commands;

namespace AMR.Forms.Fabrica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
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
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetItens(int numero, string serie, CancellationToken ct)
    {
        var result = await mediator.Send(new GetNotaFiscalItensQuery(numero, serie), ct);
        return result is null ? NotFound() : Ok(result);
    }

    // PATCH api/notafiscal/{numero}/{serie}/transmissao
    // Registra chave/protocolo da NF-e e gera ContaReceber no Financeiro
    [HttpPatch("{numero:int}/{serie}/transmissao")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RegistrarTransmissao(
        int numero, string serie,
        [FromBody] RegistrarTransmissaoRequest req,
        CancellationToken ct)
    {
        await mediator.Send(new RegistrarTransmissaoNfCommand(
            numero, serie, req.ChaveNfe, req.Protocolo, req.ValorTransmissao), ct);
        return NoContent();
    }
}

public record RegistrarTransmissaoRequest(
    string   ChaveNfe,
    string   Protocolo,
    decimal? ValorTransmissao = null
);
