namespace RDS.Forms.Fabrica.Domain.Entities;

/// <summary>
/// Itens da nota fiscal com valores tributários por produto.
/// </summary>
public class NotaFiscalDetalhe
{
    public int NumeroNotaFiscal { get; private set; }
    public string SerieNotaFiscal { get; private set; } = null!;
    public int CodigoFilial { get; private set; }
    public int? CodigoProduto { get; private set; }
    public decimal? Quantidade { get; private set; }
    public string? UnidadeMedidaComercial { get; private set; }
    public string? UnidadeMedida { get; private set; }
    public decimal? PrecoUnitario { get; private set; }
    public decimal? ValorTotal { get; private set; }
    public decimal? AliquotaIcms { get; private set; }
    public decimal? BaseReducaoIcms { get; private set; }
    public decimal? AliquotaIpi { get; private set; }
    public decimal? ValorIpi { get; private set; }
    public decimal? ValorIcmsSt { get; private set; }
    public decimal? ValorPis { get; private set; }
    public decimal? ValorCofins { get; private set; }
    public int? CodigoCfo { get; private set; }
    public string? SufixoCfo { get; private set; }
    public string? CodigoEan { get; private set; }

    public decimal? ValorTotalComIpi => ValorTotal + ValorIpi;

    protected NotaFiscalDetalhe() { }

    public NotaFiscalDetalhe(int numeroNotaFiscal, string serieNotaFiscal, int codigoFilial, int? codigoProduto, decimal? quantidade, decimal? precoUnitario)
    {
        NumeroNotaFiscal = numeroNotaFiscal;
        SerieNotaFiscal = serieNotaFiscal;
        CodigoFilial = codigoFilial;
        CodigoProduto = codigoProduto;
        Quantidade = quantidade;
        PrecoUnitario = precoUnitario;
        ValorTotal = quantidade * precoUnitario;
    }
}
