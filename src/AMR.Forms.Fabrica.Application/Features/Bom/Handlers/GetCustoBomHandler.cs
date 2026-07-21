using AMR.Forms.Fabrica.Application.Features.Bom.DTOs;
using AMR.Forms.Fabrica.Application.Features.Bom.Queries;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;
using MediatR;

namespace AMR.Forms.Fabrica.Application.Features.Bom.Handlers;

public class GetCustoBomHandler(IBomRepository bomRepo, IProdutoBomRepository produtoRepo)
    : IRequestHandler<GetCustoBomQuery, CustoBomDto?>
{
    public async Task<CustoBomDto?> Handle(GetCustoBomQuery request, CancellationToken ct)
    {
        var produto = await produtoRepo.ObterComDadosBomAsync(request.CodigoProduto);
        if (produto is null) return null;

        var (custo, componentes) = await CalcularCustoAsync(request.CodigoProduto, 1, ct);

        return new CustoBomDto(
            produto.Codigo,
            produto.Nome,
            produto.CustoPadrao,
            custo,
            componentes
        );
    }

    private async Task<(decimal Custo, IReadOnlyList<CustoBomItemDto> Componentes)> CalcularCustoAsync(
        int codigoProduto, int nivel, CancellationToken ct)
    {
        var itens = await bomRepo.ListarItensPorProdutoPaiAsync(codigoProduto, apenasAtivos: true);
        var componentesDto = new List<CustoBomItemDto>();
        decimal custoTotal = 0;

        foreach (var item in itens)
        {
            var prodFilho = await produtoRepo.ObterComDadosBomAsync(item.CodigoProdutoFilho);
            if (prodFilho is null) continue;

            decimal custoUnitario;

            if (prodFilho.TipoProduto == TiposProduto.Fabricado)
            {
                // Bottom-up: custo do fabricado = soma dos seus componentes
                var (custoFilho, _) = await CalcularCustoAsync(item.CodigoProdutoFilho, nivel + 1, ct);
                custoUnitario = custoFilho > 0 ? custoFilho : prodFilho.CustoPadrao;
            }
            else
            {
                custoUnitario = prodFilho.CustoPadrao;
            }

            var custoExtendido = custoUnitario * item.QuantidadeLiquida;
            custoTotal += custoExtendido;

            componentesDto.Add(new CustoBomItemDto(
                prodFilho.Codigo,
                prodFilho.Nome,
                prodFilho.TipoProduto,
                item.Quantidade,
                item.PercentualPerda,
                custoUnitario,
                custoExtendido,
                nivel
            ));
        }

        return (custoTotal, componentesDto.AsReadOnly());
    }
}
