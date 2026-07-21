using MediatR;
using Microsoft.AspNetCore.Mvc;
using AMR.Forms.Fabrica.Application.Features.OrdensProducao.Commands;
using AMR.Forms.Fabrica.Application.Features.OrdensProducao.Queries;
using AMR.Forms.Fabrica.Domain.Entities;

namespace AMR.Forms.Fabrica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
public class OrdensProducaoController(IMediator mediator) : ControllerBase
{
    /// <summary>Lista OPs por filial, opcionalmente filtrando por status.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int cdFilial = 1,
        [FromQuery] StatusOrdemProducao? status = null,
        CancellationToken ct = default)
        => Ok(await mediator.Send(new GetOrdensProducaoQuery(cdFilial, status), ct));

    /// <summary>Retorna uma OP por ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetOrdemProducaoQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Retorna uma OP por número.</summary>
    [HttpGet("numero/{numero}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByNumero(string numero, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetOrdemProducaoPorNumeroQuery(numero), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Retorna o histórico de rastreabilidade de uma OP.</summary>
    [HttpGet("{id:int}/rastreabilidade")]
    public async Task<IActionResult> GetRastreabilidade(int id, CancellationToken ct = default)
        => Ok(await mediator.Send(new GetRastreabilidadeQuery(id), ct));

    /// <summary>Rastreabilidade por lote (rastreio reverso).</summary>
    [HttpGet("rastreabilidade/lote/{lote}")]
    public async Task<IActionResult> GetRastreabilidadePorLote(string lote, CancellationToken ct = default)
        => Ok(await mediator.Send(new GetRastreabilidadePorLoteQuery(lote), ct));

    /// <summary>Abre uma nova Ordem de Produção (status Rascunho).</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Abrir([FromBody] AbrirOpRequest req, CancellationToken ct = default)
    {
        var cmd = new AbrirOrdemProducaoCommand(req.Numero, req.CodigoProduto, req.CodigoFilial,
            req.QuantidadePlanejada, req.DataPrevistaFim, req.Observacao);
        var result = await mediator.Send(cmd, ct);
        if (!result.Sucesso) return BadRequest(new ProblemDetails { Title = result.Erro });
        return CreatedAtAction(nameof(GetById), new { id = result.Valor }, result.Valor);
    }

    /// <summary>Libera a OP para produção.</summary>
    [HttpPut("{id:int}/liberar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Liberar(int id, CancellationToken ct = default)
    {
        var result = await mediator.Send(new LiberarOpCommand(id), ct);
        return result.Sucesso ? NoContent() : BadRequest(new ProblemDetails { Title = result.Erro });
    }

    /// <summary>Inicia a produção da OP.</summary>
    [HttpPut("{id:int}/iniciar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Iniciar(int id, CancellationToken ct = default)
    {
        var result = await mediator.Send(new IniciarProducaoCommand(id), ct);
        return result.Sucesso ? NoContent() : BadRequest(new ProblemDetails { Title = result.Erro });
    }

    /// <summary>Registra produção com rastreabilidade de lote.</summary>
    [HttpPost("{id:int}/producao")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RegistrarProducao(int id, [FromBody] RegistrarMovimentoRequest req, CancellationToken ct = default)
    {
        var cmd = new RegistrarProducaoCommand(id, req.Quantidade, req.Lote, req.CodigoOperador, req.Observacao);
        var result = await mediator.Send(cmd, ct);
        return result.Sucesso ? NoContent() : BadRequest(new ProblemDetails { Title = result.Erro });
    }

    /// <summary>Registra rejeição com rastreabilidade de lote.</summary>
    [HttpPost("{id:int}/rejeicao")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RegistrarRejeicao(int id, [FromBody] RegistrarMovimentoRequest req, CancellationToken ct = default)
    {
        var cmd = new RegistrarRejeicaoCommand(id, req.Quantidade, req.Lote, req.CodigoOperador, req.Observacao);
        var result = await mediator.Send(cmd, ct);
        return result.Sucesso ? NoContent() : BadRequest(new ProblemDetails { Title = result.Erro });
    }

    /// <summary>Conclui a OP.</summary>
    [HttpPut("{id:int}/concluir")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Concluir(int id, CancellationToken ct = default)
    {
        var result = await mediator.Send(new ConcluirOpCommand(id), ct);
        return result.Sucesso ? NoContent() : BadRequest(new ProblemDetails { Title = result.Erro });
    }

    /// <summary>Cancela a OP.</summary>
    [HttpPut("{id:int}/cancelar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Cancelar(int id, [FromBody] CancelarOpRequest req, CancellationToken ct = default)
    {
        var result = await mediator.Send(new CancelarOpCommand(id, req.Motivo), ct);
        return result.Sucesso ? NoContent() : BadRequest(new ProblemDetails { Title = result.Erro });
    }
}

public record AbrirOpRequest(string Numero, int CodigoProduto, int CodigoFilial,
    decimal QuantidadePlanejada, DateTime? DataPrevistaFim, string? Observacao);
public record RegistrarMovimentoRequest(decimal Quantidade, string? Lote, string? CodigoOperador, string? Observacao);
public record CancelarOpRequest(string Motivo);
