namespace RDS.Forms.Fabrica.Domain.Entities;

/// <summary>
/// Detalhe dos produtos efetivamente carregados/descarregados em uma ficha.
/// </summary>
public class FichaLoadDetalhe
{
    public int CodigoFicha { get; private set; }
    public int CodigoFilial { get; private set; }
    public string? CodigoBusinessUnit { get; private set; }
    public int? CodigoProduto { get; private set; }
    public decimal? Quantidade { get; private set; }
    public string? UnidadeMedida { get; private set; }
    public int? CodigoAddressNumber { get; private set; }
    public string? CodigoTipoDoctoJde { get; private set; }

    protected FichaLoadDetalhe() { }

    public FichaLoadDetalhe(int codigoFicha, int codigoFilial, int? codigoProduto, decimal? quantidade, string? unidadeMedida)
    {
        CodigoFicha = codigoFicha;
        CodigoFilial = codigoFilial;
        CodigoProduto = codigoProduto;
        Quantidade = quantidade;
        UnidadeMedida = unidadeMedida;
    }
}
