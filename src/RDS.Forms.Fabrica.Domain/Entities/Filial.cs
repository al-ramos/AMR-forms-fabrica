namespace RDS.Forms.Fabrica.Domain.Entities;

public class Filial
{
    public int Codigo { get; private set; }
    public string? Nome { get; private set; }
    public string? CodigoBuDeposito { get; private set; }
    public int? TipoImpressaoNf { get; private set; }

    protected Filial() { }

    public Filial(int codigo, string? nome, string? codigoBuDeposito, int? tipoImpressaoNf)
    {
        Codigo = codigo;
        Nome = nome;
        CodigoBuDeposito = codigoBuDeposito;
        TipoImpressaoNf = tipoImpressaoNf;
    }
}
