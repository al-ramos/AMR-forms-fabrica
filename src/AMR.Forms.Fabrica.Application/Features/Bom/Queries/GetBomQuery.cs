using AMR.Forms.Fabrica.Application.Features.Bom.DTOs;
using MediatR;

namespace AMR.Forms.Fabrica.Application.Features.Bom.Queries;

/// <summary>Retorna a árvore BOM multinível de um produto fabricado.</summary>
public record GetBomQuery(int CodigoProduto) : IRequest<BomItemDto?>;

/// <summary>Retorna a explosão de materiais (lista achatada com quantidades acumuladas).</summary>
public record GetExplosaoMaterialQuery(int CodigoProduto, decimal QuantidadeFabricar = 1) : IRequest<IReadOnlyList<ExplosaoMaterialDto>>;

/// <summary>Retorna o custo BOM calculado bottom-up.</summary>
public record GetCustoBomQuery(int CodigoProduto) : IRequest<CustoBomDto?>;
