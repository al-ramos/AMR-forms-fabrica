using MediatR;
using AMR.Forms.Fabrica.Application.Features.Pedidos.Queries;
using AMR.Forms.Fabrica.Domain.Interfaces;

namespace AMR.Forms.Fabrica.Application.Features.Pedidos.Handlers;

public class GetPedidosHandler(IPedidoRepository repo) : IRequestHandler<GetPedidosQuery, IEnumerable<PedidoDto>>
{
    public async Task<IEnumerable<PedidoDto>> Handle(GetPedidosQuery request, CancellationToken ct)
    {
        var pedidos = await repo.ListarPorFilialAsync(request.CdFilial);

        var result = new List<PedidoDto>();
        foreach (var pedido in pedidos)
        {
            var itens = await repo.ListarItensPorPedidoAsync(pedido.Codigo);
            var itensDto = itens.Select(i => new PedidoItemDto(
                i.CodigoPedido,
                i.CodigoProduto,
                i.Quantidade,
                i.UnidadeMedida));

            result.Add(new PedidoDto(
                pedido.Codigo,
                pedido.CodigoFilial,
                pedido.DataPedido,
                pedido.CodigoAddressNumber,
                itens.Sum(i => i.Quantidade ?? 0),
                itensDto));
        }

        return result;
    }
}
