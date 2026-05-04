namespace AMR.Forms.Fabrica.Domain.Entities;

/// <summary>
/// Representa uma estação de trabalho física dentro da fábrica,
/// associada a passos específicos do fluxo de operação.
/// </summary>
public class Estacao
{
    public int Codigo { get; private set; }
    public int CodigoFilial { get; private set; }
    public string? Local { get; private set; }

    protected Estacao() { }

    public Estacao(int codigo, int codigoFilial, string? local)
    {
        Codigo = codigo;
        CodigoFilial = codigoFilial;
        Local = local;
    }
}
