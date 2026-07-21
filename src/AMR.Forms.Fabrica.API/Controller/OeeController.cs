using MediatR;
using Microsoft.AspNetCore.Mvc;
using AMR.Forms.Fabrica.Application.Features.Oee.Commands;
using AMR.Forms.Fabrica.Application.Features.Oee.Queries;

namespace AMR.Forms.Fabrica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
public class EquipamentosController(IMediator mediator) : ControllerBase
{
    /// <summary>Lista equipamentos de uma filial.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int cdFilial = 1,
        [FromQuery] bool apenasAtivos = true,
        CancellationToken ct = default)
        => Ok(await mediator.Send(new GetEquipamentosQuery(cdFilial, apenasAtivos), ct));

    /// <summary>Retorna um equipamento por ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetEquipamentoQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Cadastra um novo equipamento.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Cadastrar([FromBody] CadastrarEquipamentoRequest req, CancellationToken ct = default)
    {
        var cmd = new CadastrarEquipamentoCommand(req.CodigoFilial, req.Nome, req.Descricao, req.CodigoArea);
        var result = await mediator.Send(cmd, ct);
        if (!result.Sucesso) return BadRequest(new ProblemDetails { Title = result.Erro });
        return CreatedAtAction(nameof(GetById), new { id = result.Valor }, result.Valor);
    }

    /// <summary>Atualiza dados de um equipamento.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarEquipamentoRequest req, CancellationToken ct = default)
    {
        var result = await mediator.Send(new AtualizarEquipamentoCommand(id, req.Nome, req.Descricao, req.CodigoArea), ct);
        return result.Sucesso ? NoContent() : BadRequest(new ProblemDetails { Title = result.Erro });
    }

    /// <summary>Ativa ou desativa um equipamento.</summary>
    [HttpPatch("{id:int}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AlterarStatus(int id, [FromBody] AlterarStatusRequest req, CancellationToken ct = default)
    {
        var result = await mediator.Send(new AlterarStatusEquipamentoCommand(id, req.Ativo), ct);
        return result.Sucesso ? NoContent() : BadRequest(new ProblemDetails { Title = result.Erro });
    }
}

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
public class OeeController(IMediator mediator) : ControllerBase
{
    /// <summary>Retorna um registro de OEE por ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetRegistroOeeQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Lista histórico de OEE de um equipamento (com filtro de período).</summary>
    [HttpGet("equipamento/{equipamentoId:int}")]
    public async Task<IActionResult> GetPorEquipamento(
        int equipamentoId,
        [FromQuery] DateTime? inicio = null,
        [FromQuery] DateTime? fim = null,
        CancellationToken ct = default)
        => Ok(await mediator.Send(new GetRegistrosOeePorEquipamentoQuery(equipamentoId, inicio, fim), ct));

    /// <summary>Resumo de OEE médio por equipamento em uma filial e período.</summary>
    [HttpGet("resumo")]
    public async Task<IActionResult> GetResumo(
        [FromQuery] int cdFilial = 1,
        [FromQuery] DateTime? inicio = null,
        [FromQuery] DateTime? fim = null,
        CancellationToken ct = default)
    {
        var dataInicio = inicio ?? DateTime.UtcNow.Date.AddDays(-30);
        var dataFim    = fim    ?? DateTime.UtcNow;
        return Ok(await mediator.Send(new GetOeeResumoFilialQuery(cdFilial, dataInicio, dataFim), ct));
    }

    /// <summary>Registra um período de OEE de um equipamento.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Registrar([FromBody] RegistrarOeeRequest req, CancellationToken ct = default)
    {
        var cmd = new RegistrarOeeCommand(
            req.EquipamentoId, req.CodigoFilial,
            req.DataHoraInicio, req.DataHoraFim,
            req.TempoPlanejadoMinutos, req.TempoRealProducaoMinutos,
            req.QuantidadeProduzida, req.QuantidadeAprovada,
            req.TempoCicloIdealSegundos, req.CodigoOperador, req.Observacao);

        var result = await mediator.Send(cmd, ct);
        if (!result.Sucesso) return BadRequest(new ProblemDetails { Title = result.Erro });
        return CreatedAtAction(nameof(GetById), new { id = result.Valor }, result.Valor);
    }
}

// ── Request records ───────────────────────────────────────────────────────────
public record CadastrarEquipamentoRequest(int CodigoFilial, string Nome, string? Descricao, string? CodigoArea);
public record AtualizarEquipamentoRequest(string Nome, string? Descricao, string? CodigoArea);
public record AlterarStatusRequest(bool Ativo);
public record RegistrarOeeRequest(
    int EquipamentoId,
    int CodigoFilial,
    DateTime DataHoraInicio,
    DateTime DataHoraFim,
    int TempoPlanejadoMinutos,
    int TempoRealProducaoMinutos,
    decimal QuantidadeProduzida,
    decimal QuantidadeAprovada,
    decimal TempoCicloIdealSegundos,
    string? CodigoOperador,
    string? Observacao);
