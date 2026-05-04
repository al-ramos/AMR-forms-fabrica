namespace AMR.Forms.Fabrica.Domain.Entities;

/// <summary>
/// Configuração de campos habilitados por tipo de operação,
/// controlando quais dados são coletados em cada etapa do checklist.
/// </summary>
public class TipoOperacaoPassoCfg
{
    public int CodigoFilial { get; private set; }
    public int CodigoTipoOperacao { get; private set; }
    public string? NomeTabela { get; private set; }
    public string? NomeCampo { get; private set; }
    public string? Tipo { get; private set; }
    public bool Habilitado { get; private set; }

    protected TipoOperacaoPassoCfg() { }

    public TipoOperacaoPassoCfg(int codigoFilial, int codigoTipoOperacao, string? nomeTabela, string? nomeCampo, string? tipo, int? icEnabled)
    {
        CodigoFilial = codigoFilial;
        CodigoTipoOperacao = codigoTipoOperacao;
        NomeTabela = nomeTabela;
        NomeCampo = nomeCampo;
        Tipo = tipo;
        Habilitado = icEnabled == 1;
    }
}
