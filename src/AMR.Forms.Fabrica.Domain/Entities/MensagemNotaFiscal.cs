namespace AMR.Forms.Fabrica.Domain.Entities;

/// <summary>
/// Mensagem padrão que pode ser impressa nas notas fiscais.
/// </summary>
public class MensagemNotaFiscal
{
    public int Codigo { get; private set; }
    public string? Descricao { get; private set; }

    protected MensagemNotaFiscal() { }

    public MensagemNotaFiscal(int codigo, string? descricao)
    {
        Codigo = codigo;
        Descricao = descricao;
    }
}
