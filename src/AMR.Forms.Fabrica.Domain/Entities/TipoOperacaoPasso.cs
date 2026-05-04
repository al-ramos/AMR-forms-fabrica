namespace AMR.Forms.Fabrica.Domain.Entities;

/// <summary>
/// Associação entre um TipoOperacao e seus Passos, definindo a sequência
/// do fluxo de checklist na fábrica.
/// </summary>
public class TipoOperacaoPasso
{
    public int CodigoFilial { get; private set; }
    public int CodigoTipoOperacao { get; private set; }
    public int CodigoPasso { get; private set; }
    public int? Sequencia { get; private set; }
    public int? TipoPasso { get; private set; }
    public int? CodigoPassoFlutuante { get; private set; }
    public int? CodigoPassoRetorno { get; private set; }

    protected TipoOperacaoPasso() { }

    public TipoOperacaoPasso(int codigoFilial, int codigoTipoOperacao, int codigoPasso, int? sequencia)
    {
        CodigoFilial = codigoFilial;
        CodigoTipoOperacao = codigoTipoOperacao;
        CodigoPasso = codigoPasso;
        Sequencia = sequencia;
    }
}
