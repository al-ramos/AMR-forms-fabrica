namespace AMR.Forms.Fabrica.Domain.Entities;

/// <summary>
/// Detalhe do produto por Business Unit, com unidade de medida comercial específica.
/// </summary>
public class ProdutoDetalhe
{
    public int CodigoProduto { get; private set; }
    public string CodigoBusinessUnit { get; private set; } = null!;
    public string? UnidadeMedidaComercial { get; private set; }

    protected ProdutoDetalhe() { }

    public ProdutoDetalhe(int codigoProduto, string codigoBusinessUnit, string? unidadeMedidaComercial)
    {
        CodigoProduto = codigoProduto;
        CodigoBusinessUnit = codigoBusinessUnit;
        UnidadeMedidaComercial = unidadeMedidaComercial;
    }
}
