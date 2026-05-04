namespace AMR.Forms.Fabrica.Domain.Entities;

/// <summary>
/// Representa um pedido de compra/venda vinculado às operações da fábrica.
/// </summary>
public class Pedido
{
    public int Codigo { get; private set; }
    public int CodigoFilial { get; private set; }
    public string? CodigoBusinessUnit { get; private set; }
    public int? CodigoAddressNumber { get; private set; }
    public DateOnly? DataPedido { get; private set; }
    public string? CodigoTipoDoctoJde { get; private set; }

    private readonly List<PedidoItem> _itens = new();
    public IReadOnlyCollection<PedidoItem> Itens => _itens.AsReadOnly();

    public decimal QuantidadeTotalProdutos => _itens.Sum(i => i.Quantidade ?? 0);

    protected Pedido() { }

    public Pedido(int codigo, int codigoFilial, string? codigoBusinessUnit, int? codigoAddressNumber, DateOnly? dataPedido)
    {
        Codigo = codigo;
        CodigoFilial = codigoFilial;
        CodigoBusinessUnit = codigoBusinessUnit;
        CodigoAddressNumber = codigoAddressNumber;
        DataPedido = dataPedido;
    }

    public DateTime? SincronizadoEm { get; private set; }

    public void MarcarSincronizado()
        => SincronizadoEm = DateTime.UtcNow;

    public void AdicionarItem(PedidoItem item)
    {
        _itens.Add(item);
    }
}
