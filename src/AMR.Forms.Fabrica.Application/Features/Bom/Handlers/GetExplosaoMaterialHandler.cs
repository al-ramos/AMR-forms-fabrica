using AMR.Forms.Fabrica.Application.Features.Bom.DTOs;
using AMR.Forms.Fabrica.Application.Features.Bom.Queries;
using AMR.Forms.Fabrica.Domain.Interfaces;
using MediatR;

namespace AMR.Forms.Fabrica.Application.Features.Bom.Handlers;

public class GetExplosaoMaterialHandler(IBomRepository bomRepo, IProdutoBomRepository produtoRepo)
    : IRequestHandler<GetExplosaoMaterialQuery, IReadOnlyList<ExplosaoMaterialDto>>
{
    public async Task<IReadOnlyList<ExplosaoMaterialDto>> Handle(GetExplosaoMaterialQuery request, CancellationToken ct)
    {
        var resultado = new List<ExplosaoMaterialDto>();
        await ExplodirAsync(request.CodigoProduto, request.QuantidadeFabricar, 1, resultado, ct);
        return resultado.AsReadOnly();
    }

    private async Task ExplodirAsync(int codigoProduto, decimal quantidadeAcumulada, int nivel,
        List<ExplosaoMaterialDto> resultado, CancellationToken ct)
    {
        var itens = await bomRepo.ListarItensPorProdutoPaiAsync(codigoProduto, apenasAtivos: true);

        foreach (var item in itens)
        {
            var quantidadeComPerda = item.QuantidadeLiquida * quantidadeAcumulada;
            var prodFilho = await produtoRepo.ObterComDadosBomAsync(item.CodigoProdutoFilho);

            // Agregar se já existe (mesmo produto em múltiplos caminhos)
            var existente = resultado.FirstOrDefault(r => r.CodigoProduto == item.CodigoProdutoFilho);
            if (existente is not null)
            {
                resultado.Remove(existente);
                resultado.Add(existente with
                {
                    QuantidadeTotalAcumulada = existente.QuantidadeTotalAcumulada + quantidadeComPerda
                });
            }
            else
            {
                resultado.Add(new ExplosaoMaterialDto(
                    item.CodigoProdutoFilho,
                    prodFilho?.Nome,
                    prodFilho?.CodigoProdutoLongo,
                    prodFilho?.UnidadeMedida,
                    prodFilho?.TipoProduto,
                    item.QuantidadeLiquida,
                    quantidadeComPerda,
                    item.PercentualPerda,
                    nivel
                ));
            }

            // Recursão para componentes fabricados
            if (prodFilho?.TipoProduto == "Fabricado")
                await ExplodirAsync(item.CodigoProdutoFilho, quantidadeComPerda, nivel + 1, resultado, ct);
        }
    }
}
