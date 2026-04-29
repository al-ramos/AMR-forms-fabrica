using RDS.Forms.Fabrica.Domain.Enums;

namespace RDS.Forms.Fabrica.Domain.Entities;

/// <summary>
/// Define o tipo de operação logística (expedição ou recepção) e seus passos configurados.
/// </summary>
public class TipoOperacao
{
    public int Codigo { get; private set; }
    public int CodigoFilial { get; private set; }
    public string? Nome { get; private set; }
    public TipoExpedicaoRecepcao? ExpedicaoRecepcao { get; private set; }
    public string? Interfaces { get; private set; }

    public bool EhExpedicao => ExpedicaoRecepcao == TipoExpedicaoRecepcao.Expedicao;
    public bool EhRecepcao => ExpedicaoRecepcao == TipoExpedicaoRecepcao.Recepcao;

    protected TipoOperacao() { }

    public TipoOperacao(int codigo, int codigoFilial, string? nome, string? expedicaoRecepcao, string? interfaces)
    {
        Codigo = codigo;
        CodigoFilial = codigoFilial;
        Nome = nome;
        Interfaces = interfaces;
        ExpedicaoRecepcao = expedicaoRecepcao switch
        {
            "E" => TipoExpedicaoRecepcao.Expedicao,
            "R" => TipoExpedicaoRecepcao.Recepcao,
            _ => null
        };
    }
}
