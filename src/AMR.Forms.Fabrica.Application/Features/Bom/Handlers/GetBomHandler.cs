using AMR.Forms.Fabrica.Application.Features.Bom.DTOs;
using AMR.Forms.Fabrica.Application.Features.Bom.Queries;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;
using MediatR;

namespace AMR.Forms.Fabrica.Application.Features.Bom.Handlers;

public class GetBomHandler(IBomRepository bomRepo, IProdutoBomRepository produtoRepo)
    : IRequestHandler<GetBomQuery, BomItemDto?>
{
    public async Task<BomItemDto?> Handle(GetBomQuery request, CancellationToken ct)
    {
        var produto = await produtoRepo.ObterComDadosBomAsync(request.CodigoProduto);
        if (produto is null) return null;

        var arvore = await ConstruirArvoreAsync(produto.Codigo, 1, ct);
        return arvore;
    }

    private async Task<BomItemDto?> ConstruirArvoreAsync(int codigoProduto, int nivel, CancellationToken ct)
    {
        var produto = await produtoRepo.ObterComDadosBomAsync(codigoProduto);
        if (produto is null) return null;

        var itens = await bomRepo.ListarItensPorProdutoPaiAsync(codigoProduto, apenasAtivos: true);
        var componentes = new List<BomItemDto>();

        foreach (var item in itens)
        {
            var filho = await ConstruirArvoreAsync(item.CodigoProdutoFilho, nivel + 1, ct);
            var prodFilho = await produtoRepo.ObterComDadosBomAsync(item.CodigoProdutoFilho);

            componentes.Add(new BomItemDto(
                item.CodigoProdutoPai,
                item.CodigoProdutoFilho,
                prodFilho?.Nome,
                prodFilho?.CodigoProdutoLongo,
                prodFilho?.UnidadeMedida,
                prodFilho?.TipoProduto,
                item.Quantidade,
                item.PercentualPerda,
                item.QuantidadeLiquida,
                item.Nivel,
                item.Ativo,
                filho?.Componentes ?? []
            ));
        }

        return new BomItemDto(
            0,
            produto.Codigo,
            produto.Nome,
            produto.CodigoProdutoLongo,
            produto.UnidadeMedida,
            produto.TipoProduto,
            1,
            0,
            1,
            nivel,
            true,
            componentes
        );
    }
}
