using MediatR;

namespace RDS.Forms.Fabrica.Application.Features.Pedidos.Queries;

public record GetPedidosQuery(int CdFilial) : IRequest<IEnumerable<PedidoDto>>;

public record PedidoDto(
    int Codigo,
    int CodigoFilial,
    DateOnly? DataPedido,
    int? CodigoAddressNumber,
    decimal QuantidadeTotalProdutos,
    IEnumerable<PedidoItemDto> Itens);

public record PedidoItemDto(
    int CodigoPedido,
    int? CodigoProduto,
    decimal? Quantidade,
    string? UnidadeMedida);
