namespace AMR.Forms.Fabrica.Domain.Entities;

/// <summary>
/// Representa um produto que transita pelas operações da fábrica.
/// </summary>
public class Produto
{
    public int Codigo { get; private set; }
    public string? CodigoBusinessUnit { get; private set; }
    public string? CodigoProdutoLongo { get; private set; }
    public string? Nome { get; private set; }
    public string? CodigoEan { get; private set; }
    public string? UnidadeMedidaComercial { get; private set; }
    public string? UnidadeMedida { get; private set; }
    public string? CodigoCtf { get; private set; }
    public string? CodigoClf { get; private set; }

    protected Produto() { }

    public Produto(int codigo, string? nome, string? codigoProdutoLongo, string? codigoEan, string? unidadeMedida)
    {
        if (codigo <= 0)
            throw new ArgumentException("Código do produto deve ser positivo.", nameof(codigo));

        Codigo = codigo;
        Nome = nome;
        CodigoProdutoLongo = codigoProdutoLongo;
        CodigoEan = codigoEan;
        UnidadeMedida = unidadeMedida;
    }
}
