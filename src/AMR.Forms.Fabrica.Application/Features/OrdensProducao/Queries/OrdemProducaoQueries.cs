using AMR.Forms.Fabrica.Application.Features.OrdensProducao.DTOs;
using AMR.Forms.Fabrica.Domain.Entities;
using MediatR;

namespace AMR.Forms.Fabrica.Application.Features.OrdensProducao.Queries;

public record GetOrdemProducaoQuery(int Id) : IRequest<OrdemProducaoDto?>;
public record GetOrdemProducaoPorNumeroQuery(string Numero) : IRequest<OrdemProducaoDto?>;
public record GetOrdensProducaoQuery(int CodigoFilial, StatusOrdemProducao? Status = null) : IRequest<IReadOnlyList<OrdemProducaoDto>>;
public record GetRastreabilidadeQuery(int OrdemProducaoId) : IRequest<IReadOnlyList<RastreabilidadeItemDto>>;
public record GetRastreabilidadePorLoteQuery(string Lote) : IRequest<IReadOnlyList<RastreabilidadeItemDto>>;
