using AMR.Forms.Fabrica.Application.Features.Manutencao.DTOs;
using AMR.Forms.Fabrica.Application.Features.Manutencao.Queries;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;
using MediatR;

namespace AMR.Forms.Fabrica.Application.Features.Manutencao.Handlers;

public class GetPlanoManutencaoHandler(IPlanoManutencaoRepository repo, IEquipamentoRepository equipRepo)
    : IRequestHandler<GetPlanoManutencaoQuery, PlanoManutencaoDto?>
{
    public async Task<PlanoManutencaoDto?> Handle(GetPlanoManutencaoQuery request, CancellationToken ct)
    {
        var plano = await repo.ObterPorIdAsync(request.Id);
        if (plano is null) return null;
        var eq = await equipRepo.ObterPorIdAsync(plano.EquipamentoId);
        return MappingExtensions.ToDto(plano, eq?.Nome);
    }
}

public class GetPlanosManutencaoHandler(IPlanoManutencaoRepository repo, IEquipamentoRepository equipRepo)
    : IRequestHandler<GetPlanosManutencaoQuery, IReadOnlyList<PlanoManutencaoDto>>
{
    public async Task<IReadOnlyList<PlanoManutencaoDto>> Handle(GetPlanosManutencaoQuery request, CancellationToken ct)
    {
        var eq = await equipRepo.ObterPorIdAsync(request.EquipamentoId);
        var planos = await repo.ListarPorEquipamentoAsync(request.EquipamentoId, request.ApenasAtivos);
        return planos.Select(p => MappingExtensions.ToDto(p, eq?.Nome)).ToList().AsReadOnly();
    }
}

public class GetPlanosVencidosOuProximosHandler(IPlanoManutencaoRepository repo, IEquipamentoRepository equipRepo)
    : IRequestHandler<GetPlanosVencidosOuProximosQuery, IReadOnlyList<PlanoManutencaoDto>>
{
    public async Task<IReadOnlyList<PlanoManutencaoDto>> Handle(GetPlanosVencidosOuProximosQuery request, CancellationToken ct)
    {
        var planos = await repo.ListarVencidosOuProximosAsync(request.CodigoFilial, request.DiasAntecedencia);
        var result = new List<PlanoManutencaoDto>();
        foreach (var p in planos)
        {
            var eq = await equipRepo.ObterPorIdAsync(p.EquipamentoId);
            result.Add(MappingExtensions.ToDto(p, eq?.Nome));
        }
        return result.AsReadOnly();
    }
}

public class GetOrdemManutencaoHandler(IOrdemManutencaoRepository repo, IEquipamentoRepository equipRepo)
    : IRequestHandler<GetOrdemManutencaoQuery, OrdemManutencaoDto?>
{
    public async Task<OrdemManutencaoDto?> Handle(GetOrdemManutencaoQuery request, CancellationToken ct)
    {
        var om = await repo.ObterPorIdAsync(request.Id);
        if (om is null) return null;
        var eq = await equipRepo.ObterPorIdAsync(om.EquipamentoId);
        return MappingExtensions.ToDto(om, eq?.Nome);
    }
}

public class GetOrdensManutencaoHandler(IOrdemManutencaoRepository repo, IEquipamentoRepository equipRepo)
    : IRequestHandler<GetOrdensManutencaoQuery, IReadOnlyList<OrdemManutencaoDto>>
{
    public async Task<IReadOnlyList<OrdemManutencaoDto>> Handle(GetOrdensManutencaoQuery request, CancellationToken ct)
    {
        var ordens = await repo.ListarPorFilialAsync(request.CodigoFilial, request.Status);
        var result = new List<OrdemManutencaoDto>();
        foreach (var om in ordens)
        {
            var eq = await equipRepo.ObterPorIdAsync(om.EquipamentoId);
            result.Add(MappingExtensions.ToDto(om, eq?.Nome));
        }
        return result.AsReadOnly();
    }
}

public class GetOrdensManutencaoPorEquipamentoHandler(IOrdemManutencaoRepository repo, IEquipamentoRepository equipRepo)
    : IRequestHandler<GetOrdensManutencaoPorEquipamentoQuery, IReadOnlyList<OrdemManutencaoDto>>
{
    public async Task<IReadOnlyList<OrdemManutencaoDto>> Handle(GetOrdensManutencaoPorEquipamentoQuery request, CancellationToken ct)
    {
        var eq = await equipRepo.ObterPorIdAsync(request.EquipamentoId);
        var ordens = await repo.ListarPorEquipamentoAsync(request.EquipamentoId, request.Status);
        return ordens.Select(om => MappingExtensions.ToDto(om, eq?.Nome)).ToList().AsReadOnly();
    }
}

// ── helpers ──────────────────────────────────────────────────────────────────
file static class MappingExtensions
{
    internal static PlanoManutencaoDto ToDto(PlanoManutencao p, string? nomeEquipamento) => new(
        p.Id, p.EquipamentoId, nomeEquipamento, p.CodigoFilial,
        p.TipoManutencao, p.Descricao, p.Instrucoes,
        p.FrequenciaDias, p.DuracaoEstimadaHoras,
        p.ProximaExecucao, p.UltimaExecucao, p.Ativo, p.CriadoEm);

    internal static OrdemManutencaoDto ToDto(OrdemManutencao om, string? nomeEquipamento) => new(
        om.Id, om.PlanoManutencaoId, om.EquipamentoId, nomeEquipamento, om.CodigoFilial,
        om.TipoManutencao, om.Descricao, om.Status,
        om.DataPrevista, om.DataInicio, om.DataConclusao,
        om.DuracaoRealHoras, om.AtrasoEmDias,
        om.CodigoTecnico, om.Observacao, om.MotivoCancelamento, om.CriadoEm);
}
