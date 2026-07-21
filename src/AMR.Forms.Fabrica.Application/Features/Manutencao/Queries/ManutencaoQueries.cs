using AMR.Forms.Fabrica.Application.Features.Manutencao.DTOs;
using AMR.Forms.Fabrica.Domain.Entities;
using MediatR;

namespace AMR.Forms.Fabrica.Application.Features.Manutencao.Queries;

public record GetPlanoManutencaoQuery(int Id) : IRequest<PlanoManutencaoDto?>;

public record GetPlanosManutencaoQuery(int EquipamentoId, bool ApenasAtivos = true)
    : IRequest<IReadOnlyList<PlanoManutencaoDto>>;

public record GetPlanosVencidosOuProximosQuery(int CodigoFilial, int DiasAntecedencia = 7)
    : IRequest<IReadOnlyList<PlanoManutencaoDto>>;

public record GetOrdemManutencaoQuery(int Id) : IRequest<OrdemManutencaoDto?>;

public record GetOrdensManutencaoQuery(
    int CodigoFilial,
    StatusOrdemManutencao? Status = null
) : IRequest<IReadOnlyList<OrdemManutencaoDto>>;

public record GetOrdensManutencaoPorEquipamentoQuery(
    int EquipamentoId,
    StatusOrdemManutencao? Status = null
) : IRequest<IReadOnlyList<OrdemManutencaoDto>>;
