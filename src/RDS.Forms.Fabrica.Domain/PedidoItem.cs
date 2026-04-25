namespace RDS.Forms.Fabrica.Domain.Entities;

/// <summary>
/// Item de um pedido com produto e quantidade.
/// </summary>
public class PedidoItem
{
    public int CodigoPedido { get; private set; }
    public int CodigoFilial { get; private set; }
    public int? CodigoProduto { get; private set; }
    public decimal? Quantidade { get; private set; }
    public string? UnidadeMedida { get; private set; }
    public string? CodigoBusinessUnit { get; private set; }
    public int? CodigoAddressNumber { get; private set; }
    public string? CodigoTipoDoctoJde { get; private set; }

    protected PedidoItem() { }

    public PedidoItem(int codigoPedido, int codigoFilial, int? codigoProduto, decimal? quantidade, string? unidadeMedida)
    {
        CodigoPedido = codigoPedido;
        CodigoFilial = codigoFilial;
        CodigoProduto = codigoProduto;
        Quantidade = quantidade;
        UnidadeMedida = unidadeMedida;
    }
}
