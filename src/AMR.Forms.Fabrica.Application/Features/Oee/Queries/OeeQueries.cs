using AMR.Forms.Fabrica.Application.Features.Oee.DTOs;
using MediatR;

namespace AMR.Forms.Fabrica.Application.Features.Oee.Queries;

/// <summary>Retorna um equipamento por ID.</summary>
public record GetEquipamentoQuery(int Id) : IRequest<EquipamentoDto?>;

/// <summary>Lista equipamentos de uma filial.</summary>
public record GetEquipamentosQuery(int CodigoFilial, bool ApenasAtivos = true)
    : IRequest<IReadOnlyList<EquipamentoDto>>;

/// <summary>Retorna um registro de OEE por ID.</summary>
public record GetRegistroOeeQuery(int Id) : IRequest<RegistroOeeDto?>;

/// <summary>Lista registros de OEE de um equipamento, com filtro de período opcional.</summary>
public record GetRegistrosOeePorEquipamentoQuery(
    int EquipamentoId,
    DateTime? Inicio = null,
    DateTime? Fim = null
) : IRequest<IReadOnlyList<RegistroOeeDto>>;

/// <summary>Resumo de OEE (médias) de uma filial num período.</summary>
public record GetOeeResumoFilialQuery(
    int CodigoFilial,
    DateTime DataInicio,
    DateTime DataFim
) : IRequest<IReadOnlyList<OeeResumoDto>>;
