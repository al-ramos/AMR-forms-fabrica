using AMR.Forms.Fabrica.Application.Features.Oee.DTOs;
using AMR.Forms.Fabrica.Application.Features.Oee.Queries;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;
using MediatR;

namespace AMR.Forms.Fabrica.Application.Features.Oee.Handlers;

public class GetEquipamentoHandler(IEquipamentoRepository repo)
    : IRequestHandler<GetEquipamentoQuery, EquipamentoDto?>
{
    public async Task<EquipamentoDto?> Handle(GetEquipamentoQuery request, CancellationToken ct)
    {
        var eq = await repo.ObterPorIdAsync(request.Id);
        return eq is null ? null : MappingExtensions.ToDto(eq);
    }
}

public class GetEquipamentosHandler(IEquipamentoRepository repo)
    : IRequestHandler<GetEquipamentosQuery, IReadOnlyList<EquipamentoDto>>
{
    public async Task<IReadOnlyList<EquipamentoDto>> Handle(GetEquipamentosQuery request, CancellationToken ct)
    {
        var lista = await repo.ListarPorFilialAsync(request.CodigoFilial, request.ApenasAtivos);
        return lista.Select(MappingExtensions.ToDto).ToList().AsReadOnly();
    }
}

public class GetRegistroOeeHandler(IRegistroOeeRepository repo, IEquipamentoRepository equipRepo)
    : IRequestHandler<GetRegistroOeeQuery, RegistroOeeDto?>
{
    public async Task<RegistroOeeDto?> Handle(GetRegistroOeeQuery request, CancellationToken ct)
    {
        var reg = await repo.ObterPorIdAsync(request.Id);
        if (reg is null) return null;
        var eq = await equipRepo.ObterPorIdAsync(reg.EquipamentoId);
        return MappingExtensions.ToDto(reg, eq?.Nome);
    }
}

public class GetRegistrosOeePorEquipamentoHandler(IRegistroOeeRepository repo, IEquipamentoRepository equipRepo)
    : IRequestHandler<GetRegistrosOeePorEquipamentoQuery, IReadOnlyList<RegistroOeeDto>>
{
    public async Task<IReadOnlyList<RegistroOeeDto>> Handle(GetRegistrosOeePorEquipamentoQuery request, CancellationToken ct)
    {
        var eq = await equipRepo.ObterPorIdAsync(request.EquipamentoId);
        var registros = await repo.ListarPorEquipamentoAsync(request.EquipamentoId, request.Inicio, request.Fim);
        return registros.Select(r => MappingExtensions.ToDto(r, eq?.Nome)).ToList().AsReadOnly();
    }
}

public class GetOeeResumoFilialHandler(IRegistroOeeRepository repo, IEquipamentoRepository equipRepo)
    : IRequestHandler<GetOeeResumoFilialQuery, IReadOnlyList<OeeResumoDto>>
{
    public async Task<IReadOnlyList<OeeResumoDto>> Handle(GetOeeResumoFilialQuery request, CancellationToken ct)
    {
        var registros = await repo.ListarPorFilialAsync(request.CodigoFilial, request.DataInicio, request.DataFim);

        var agrupado = registros
            .GroupBy(r => r.EquipamentoId)
            .Select(g => new OeeResumoDto(
                g.Key,
                null, // nome resolvido abaixo
                request.DataInicio,
                request.DataFim,
                g.Count(),
                g.Any() ? Math.Round(g.Average(r => r.Disponibilidade), 4) : 0m,
                g.Any() ? Math.Round(g.Average(r => r.Performance), 4) : 0m,
                g.Any() ? Math.Round(g.Average(r => r.Qualidade), 4) : 0m,
                g.Any() ? Math.Round(g.Average(r => r.Oee), 4) : 0m
            ))
            .ToList();

        // Resolve nomes dos equipamentos
        var result = new List<OeeResumoDto>();
        foreach (var item in agrupado)
        {
            var eq = await equipRepo.ObterPorIdAsync(item.EquipamentoId);
            result.Add(item with { NomeEquipamento = eq?.Nome });
        }

        return result.AsReadOnly();
    }
}

// ── helpers ──────────────────────────────────────────────────────────────────
file static class MappingExtensions
{
    internal static EquipamentoDto ToDto(Equipamento eq) => new(
        eq.Id, eq.CodigoFilial, eq.Nome, eq.Descricao, eq.CodigoArea, eq.Ativo, eq.CriadoEm);

    internal static RegistroOeeDto ToDto(RegistroOee r, string? nomeEquipamento) => new(
        r.Id, r.EquipamentoId, nomeEquipamento, r.CodigoFilial,
        r.DataHoraInicio, r.DataHoraFim,
        r.TempoPlanejadoMinutos, r.TempoRealProducaoMinutos,
        r.QuantidadeProduzida, r.QuantidadeAprovada, r.TempoCicloIdealSegundos,
        r.Disponibilidade, r.Performance, r.Qualidade, r.Oee,
        r.CodigoOperador, r.Observacao, r.CriadoEm);
}
