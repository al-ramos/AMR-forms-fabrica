using MediatR;
using Microsoft.AspNetCore.Mvc;
using AMR.Forms.Fabrica.Application.Features.Manutencao.Commands;
using AMR.Forms.Fabrica.Application.Features.Manutencao.Queries;
using AMR.Forms.Fabrica.Domain.Entities;

namespace AMR.Forms.Fabrica.API.Controllers;

// ── Planos de Manutenção ──────────────────────────────────────────────────────

[ApiController]
[Route("api/planos-manutencao")]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
public class PlanosManutencaoController(IMediator mediator) : ControllerBase
{
    /// <summary>Lista planos de manutenção de um equipamento.</summary>
    [HttpGet("equipamento/{equipamentoId:int}")]
    public async Task<IActionResult> GetPorEquipamento(
        int equipamentoId, [FromQuery] bool apenasAtivos = true, CancellationToken ct = default)
        => Ok(await mediator.Send(new GetPlanosManutencaoQuery(equipamentoId, apenasAtivos), ct));

    /// <summary>Lista planos vencidos ou que vencem nos próximos N dias.</summary>
    [HttpGet("proximos")]
    public async Task<IActionResult> GetVencidosOuProximos(
        [FromQuery] int cdFilial = 1, [FromQuery] int diasAntecedencia = 7, CancellationToken ct = default)
        => Ok(await mediator.Send(new GetPlanosVencidosOuProximosQuery(cdFilial, diasAntecedencia), ct));

    /// <summary>Retorna um plano por ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetPlanoManutencaoQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Cria um novo plano de manutenção.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Criar([FromBody] CriarPlanoRequest req, CancellationToken ct = default)
    {
        var cmd = new CriarPlanoManutencaoCommand(
            req.EquipamentoId, req.CodigoFilial, req.TipoManutencao,
            req.Descricao, req.Instrucoes, req.FrequenciaDias,
            req.DuracaoEstimadaHoras, req.ProximaExecucao);
        var result = await mediator.Send(cmd, ct);
        if (!result.Sucesso) return BadRequest(new ProblemDetails { Title = result.Erro });
        return CreatedAtAction(nameof(GetById), new { id = result.Valor }, result.Valor);
    }

    /// <summary>Atualiza dados de um plano.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarPlanoRequest req, CancellationToken ct = default)
    {
        var result = await mediator.Send(new AtualizarPlanoManutencaoCommand(
            id, req.Descricao, req.Instrucoes, req.FrequenciaDias, req.DuracaoEstimadaHoras), ct);
        return result.Sucesso ? NoContent() : BadRequest(new ProblemDetails { Title = result.Erro });
    }

    /// <summary>Ativa ou desativa um plano.</summary>
    [HttpPatch("{id:int}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AlterarStatus(int id, [FromBody] AlterarStatusPlanoRequest req, CancellationToken ct = default)
    {
        var result = await mediator.Send(new AlterarStatusPlanoCommand(id, req.Ativo), ct);
        return result.Sucesso ? NoContent() : BadRequest(new ProblemDetails { Title = result.Erro });
    }
}

// ── Ordens de Manutenção ──────────────────────────────────────────────────────

[ApiController]
[Route("api/ordens-manutencao")]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
public class OrdensManutencaoController(IMediator mediator) : ControllerBase
{
    /// <summary>Lista OMs de uma filial, com filtro de status.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int cdFilial = 1,
        [FromQuery] StatusOrdemManutencao? status = null,
        CancellationToken ct = default)
        => Ok(await mediator.Send(new GetOrdensManutencaoQuery(cdFilial, status), ct));

    /// <summary>Lista OMs de um equipamento.</summary>
    [HttpGet("equipamento/{equipamentoId:int}")]
    public async Task<IActionResult> GetPorEquipamento(
        int equipamentoId, [FromQuery] StatusOrdemManutencao? status = null, CancellationToken ct = default)
        => Ok(await mediator.Send(new GetOrdensManutencaoPorEquipamentoQuery(equipamentoId, status), ct));

    /// <summary>Retorna uma OM por ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetOrdemManutencaoQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Abre uma nova Ordem de Manutenção.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Abrir([FromBody] AbrirOmRequest req, CancellationToken ct = default)
    {
        var cmd = new AbrirOrdemManutencaoCommand(
            req.EquipamentoId, req.CodigoFilial, req.TipoManutencao,
            req.Descricao, req.DataPrevista, req.CodigoTecnico,
            req.Observacao, req.PlanoManutencaoId);
        var result = await mediator.Send(cmd, ct);
        if (!result.Sucesso) return BadRequest(new ProblemDetails { Title = result.Erro });
        return CreatedAtAction(nameof(GetById), new { id = result.Valor }, result.Valor);
    }

    /// <summary>Inicia execução da OM.</summary>
    [HttpPut("{id:int}/iniciar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Iniciar(int id, [FromBody] IniciarOmRequest req, CancellationToken ct = default)
    {
        var result = await mediator.Send(new IniciarOrdemManutencaoCommand(id, req.CodigoTecnico), ct);
        return result.Sucesso ? NoContent() : BadRequest(new ProblemDetails { Title = result.Erro });
    }

    /// <summary>Conclui a OM registrando duração real.</summary>
    [HttpPut("{id:int}/concluir")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Concluir(int id, [FromBody] ConcluirOmRequest req, CancellationToken ct = default)
    {
        var result = await mediator.Send(new ConcluirOrdemManutencaoCommand(id, req.DuracaoRealHoras, req.Observacao), ct);
        return result.Sucesso ? NoContent() : BadRequest(new ProblemDetails { Title = result.Erro });
    }

    /// <summary>Cancela a OM.</summary>
    [HttpPut("{id:int}/cancelar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Cancelar(int id, [FromBody] CancelarOmRequest req, CancellationToken ct = default)
    {
        var result = await mediator.Send(new CancelarOrdemManutencaoCommand(id, req.Motivo), ct);
        return result.Sucesso ? NoContent() : BadRequest(new ProblemDetails { Title = result.Erro });
    }
}

// ── Request records ───────────────────────────────────────────────────────────
public record CriarPlanoRequest(
    int EquipamentoId, int CodigoFilial, TipoManutencao TipoManutencao,
    string Descricao, string? Instrucoes, int FrequenciaDias,
    decimal DuracaoEstimadaHoras, DateTime ProximaExecucao);

public record AtualizarPlanoRequest(
    string Descricao, string? Instrucoes, int FrequenciaDias, decimal DuracaoEstimadaHoras);

public record AlterarStatusPlanoRequest(bool Ativo);

public record AbrirOmRequest(
    int EquipamentoId, int CodigoFilial, TipoManutencao TipoManutencao,
    string Descricao, DateTime DataPrevista, string? CodigoTecnico,
    string? Observacao, int? PlanoManutencaoId = null);

public record IniciarOmRequest(string? CodigoTecnico);
public record ConcluirOmRequest(decimal DuracaoRealHoras, string? Observacao);
public record CancelarOmRequest(string Motivo);
