using AMR.Forms.Fabrica.Application.Features.Bom.DTOs;
using AMR.Forms.Fabrica.Application.Features.Bom.Queries;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;
using MediatR;

namespace AMR.Forms.Fabrica.Application.Features.Bom.Handlers;

public class GetExplosaoMaterialHandler(IBomRepository bomRepo, IProdutoBomRepository produtoRepo)
    : IRequestHandler<GetExplosaoMaterialQuery, IReadOnlyList<ExplosaoMaterialDto>>
{
    public async Task<IReadOnlyList<ExplosaoMaterialDto>> Handle(GetExplosaoMaterialQuery request, CancellationToken ct)
    {
        // Dictionary garante agregação O(1) quando o mesmo componente aparece em múltiplos caminhos
        var acumulado = new Dictionary<int, ExplosaoMaterialDto>();
        await ExplodirAsync(request.CodigoProduto, request.QuantidadeFabricar, 1, acumulado, ct);
        return acumulado.Values.ToList().AsReadOnly();
    }

    private async Task ExplodirAsync(int codigoProduto, decimal quantidadeAcumulada, int nivel,
        Dictionary<int, ExplosaoMaterialDto> acumulado, CancellationToken ct)
    {
        var itens = await bomRepo.ListarItensPorProdutoPaiAsync(codigoProduto, apenasAtivos: true);

        foreach (var item in itens)
        {
            var quantidadeComPerda = item.QuantidadeLiquida * quantidadeAcumulada;
            var prodFilho = await produtoRepo.ObterComDadosBomAsync(item.CodigoProdutoFilho);

            if (acumulado.TryGetValue(item.CodigoProdutoFilho, out var existente))
            {
                // Mesmo componente em caminho diferente: acumula a quantidade
                acumulado[item.CodigoProdutoFilho] = existente with
                {
                    QuantidadeTotalAcumulada = existente.QuantidadeTotalAcumulada + quantidadeComPerda
                };
            }
            else
            {
                acumulado[item.CodigoProdutoFilho] = new ExplosaoMaterialDto(
                    item.CodigoProdutoFilho,
                    prodFilho?.Nome,
                    prodFilho?.CodigoProdutoLongo,
                    prodFilho?.UnidadeMedida,
                    prodFilho?.TipoProduto,
                    item.QuantidadeLiquida,
                    quantidadeComPerda,
                    item.PercentualPerda,
                    nivel
                );
            }

            // Recursão apenas para subprodutos fabricados
            if (prodFilho?.TipoProduto == TiposProduto.Fabricado)
                await ExplodirAsync(item.CodigoProdutoFilho, quantidadeComPerda, nivel + 1, acumulado, ct);
        }
    }
}
