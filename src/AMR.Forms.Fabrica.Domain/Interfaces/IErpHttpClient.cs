namespace AMR.Forms.Fabrica.Domain.Interfaces;

public interface IErpHttpClient
{
    Task<IEnumerable<PedidoErpDto>> ObterPedidosAprovadosAsync(int codigoFilial, CancellationToken ct = default);
}

public record PedidoErpDto(
    int Codigo,
    int CodigoFilial,
    DateOnly? DataPedido,
    int? CodigoAddressNumber,
    decimal QuantidadeTotalProdutos,
    IEnumerable<PedidoItemErpDto> Itens
);

public record PedidoItemErpDto(
    int CodigoPedido,
    int? CodigoProduto,
    decimal? Quantidade,
    string? UnidadeMedida
);
