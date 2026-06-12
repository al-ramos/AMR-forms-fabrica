using AMR.Forms.Fabrica.Domain.Entities;

namespace AMR.Forms.Fabrica.Tests.Domain;

public class PedidoTests
{
    private static Pedido CriarPedido(int codigo = 1)
        => new(codigo, codigoFilial: 1, codigoBusinessUnit: "BU01", codigoAddressNumber: 100,
               dataPedido: DateOnly.FromDateTime(DateTime.Today));

    private static PedidoItem CriarItem(int codigoPedido = 1, decimal? quantidade = 10m)
        => new(codigoPedido, codigoFilial: 1, codigoProduto: 5, quantidade, unidadeMedida: "UN");

    // ── QuantidadeTotalProdutos ───────────────────────────────────────────────

    [Fact]
    public void QuantidadeTotalProdutos_SemItens_RetornaZero()
    {
        var pedido = CriarPedido();
        Assert.Equal(0m, pedido.QuantidadeTotalProdutos);
    }

    [Fact]
    public void QuantidadeTotalProdutos_ComUmItem_RetornaQuantidadeDoItem()
    {
        var pedido = CriarPedido();
        pedido.AdicionarItem(CriarItem(quantidade: 15m));
        Assert.Equal(15m, pedido.QuantidadeTotalProdutos);
    }

    [Fact]
    public void QuantidadeTotalProdutos_ComMultiplosItens_RetornaSoma()
    {
        var pedido = CriarPedido();
        pedido.AdicionarItem(CriarItem(quantidade: 10m));
        pedido.AdicionarItem(CriarItem(quantidade: 5m));
        pedido.AdicionarItem(CriarItem(quantidade: 3m));
        Assert.Equal(18m, pedido.QuantidadeTotalProdutos);
    }

    // ── AdicionarItem ─────────────────────────────────────────────────────────

    [Fact]
    public void AdicionarItem_IncrementaColecaoDeItens()
    {
        var pedido = CriarPedido();
        pedido.AdicionarItem(CriarItem());
        pedido.AdicionarItem(CriarItem());
        Assert.Equal(2, pedido.Itens.Count);
    }

    // ── MarcarSincronizado ────────────────────────────────────────────────────

    [Fact]
    public void MarcarSincronizado_SetaSincronizadoEm()
    {
        var pedido = CriarPedido();
        Assert.Null(pedido.SincronizadoEm);
        pedido.MarcarSincronizado();
        Assert.NotNull(pedido.SincronizadoEm);
    }

    [Fact]
    public void MarcarSincronizado_DataEmUtc()
    {
        var pedido = CriarPedido();
        pedido.MarcarSincronizado();
        Assert.Equal(DateTimeKind.Utc, pedido.SincronizadoEm!.Value.Kind);
    }

    // ── Construtor ────────────────────────────────────────────────────────────

    [Fact]
    public void Construtor_PreencheCamposCorretamente()
    {
        var data = DateOnly.FromDateTime(DateTime.Today);
        var pedido = new Pedido(77, 3, "BU99", 200, data);

        Assert.Equal(77, pedido.Codigo);
        Assert.Equal(3, pedido.CodigoFilial);
        Assert.Equal("BU99", pedido.CodigoBusinessUnit);
        Assert.Equal(200, pedido.CodigoAddressNumber);
        Assert.Equal(data, pedido.DataPedido);
        Assert.Empty(pedido.Itens);
    }
}
