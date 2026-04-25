namespace RDS.Forms.Fabrica.Domain.Entities;

/// <summary>
/// Departamento da fábrica, agrupador de produtos para controle operacional.
/// </summary>
public class Departamento
{
    public int Codigo { get; private set; }
    public int CodigoFilial { get; private set; }
    public string? DescricaoProduto { get; private set; }

    protected Departamento() { }

    public Departamento(int codigo, int codigoFilial, string? descricaoProduto)
    {
        Codigo = codigo;
        CodigoFilial = codigoFilial;
        DescricaoProduto = descricaoProduto;
    }
}
