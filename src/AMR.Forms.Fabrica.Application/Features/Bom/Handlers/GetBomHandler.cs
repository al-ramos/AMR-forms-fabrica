using AMR.Forms.Fabrica.Application.Features.Bom.DTOs;
using AMR.Forms.Fabrica.Application.Features.Bom.Queries;
using AMR.Forms.Fabrica.Domain.Interfaces;
using MediatR;

namespace AMR.Forms.Fabrica.Application.Features.Bom.Handlers;

public class GetBomHandler(IBomRepository bomRepo, IProdutoBomRepository produtoRepo)
    : IRequestHandler<GetBomQuery, BomItemDto?>
{
    // Uma única entrada — a recursão já busca o produto do nó filho internamente,
    // então não duplicamos a chamada a ObterComDadosBomAsync.
    public async Task<BomItemDto?> Handle(GetBomQuery request, CancellationToken ct)
        => await ConstruirArvoreAsync(request.CodigoProduto, 1, ct);

    private async Task<BomItemDto?> ConstruirArvoreAsync(int codigoProduto, int nivel, CancellationToken ct)
    {
        var produto = await produtoRepo.ObterComDadosBomAsync(codigoProduto);
        if (produto is null) return null;

        var itens = await bomRepo.ListarItensPorProdutoPaiAsync(codigoProduto, apenasAtivos: true);
        var componentes = new List<BomItemDto>();

        foreach (var item in itens)
        {
            // A recursão já traz os dados do produto filho — reutilizamos do DTO retornado
            // em vez de chamar ObterComDadosBomAsync novamente para cada filho.
            var filho = await ConstruirArvoreAsync(item.CodigoProdutoFilho, nivel + 1, ct);

            componentes.Add(new BomItemDto(
                item.CodigoProdutoPai,
                item.CodigoProdutoFilho,
                filho?.NomeProdutoFilho,
                filho?.CodigoProdutoLongo,
                filho?.UnidadeMedida,
                filho?.TipoProduto,
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
