namespace AMR.Forms.Fabrica.Domain.Entities;

/// <summary>
/// Natureza de Operação (CFOP) utilizada na emissão de notas fiscais.
/// </summary>
public class NaturezaOperacao
{
    public int CodigoCfo { get; private set; }
    public string SufixoCfo { get; private set; } = null!;
    public string? Descricao { get; private set; }

    protected NaturezaOperacao() { }

    public NaturezaOperacao(int codigoCfo, string sufixoCfo, string? descricao)
    {
        CodigoCfo = codigoCfo;
        SufixoCfo = sufixoCfo;
        Descricao = descricao;
    }
}
