namespace RDS.Forms.Fabrica.Domain.Entities;

/// <summary>
/// Representa a Nota Fiscal eletrônica gerada para uma operação (ficha).
/// </summary>
public class NotaFiscal
{
    public int Numero { get; private set; }
    public string SerieNotaFiscal { get; private set; } = null!;
    public int CodigoFilial { get; private set; }
    public int? CodigoFicha { get; private set; }
    public string? CodigoBusinessUnit { get; private set; }
    public DateOnly? DataEmissao { get; private set; }
    public string? ChaveNfe { get; private set; }
    public string? Protocolo { get; private set; }
    public string? Ambiente { get; private set; }
    public string? ModeloNf { get; private set; }
    public string? NomeFilial { get; private set; }
    public string? NomeCliente { get; private set; }
    public string? CnpjCliente { get; private set; }
    public int? Impressoes { get; private set; }
    public int? Cancelado { get; private set; }
    public decimal? ValorTransmissao { get; private set; }

    public bool EstaCancelada => Cancelado == 1;
    public bool EhAmbienteProducao => Ambiente == "1";
    public bool FoiTransmitida => !string.IsNullOrEmpty(ChaveNfe) && !string.IsNullOrEmpty(Protocolo);

    protected NotaFiscal() { }

    public NotaFiscal(int numero, string serieNotaFiscal, int codigoFilial, int? codigoFicha, DateOnly? dataEmissao)
    {
        Numero = numero;
        SerieNotaFiscal = serieNotaFiscal;
        CodigoFilial = codigoFilial;
        CodigoFicha = codigoFicha;
        DataEmissao = dataEmissao;
    }

    public void RegistrarTransmissao(string chaveNfe, string protocolo, decimal? valorTransmissao)
    {
        ChaveNfe = chaveNfe;
        Protocolo = protocolo;
        ValorTransmissao = valorTransmissao;
    }

    public void Cancelar(string justificativa)
    {
        if (EstaCancelada)
            throw new InvalidOperationException("Nota fiscal já está cancelada.");

        Cancelado = 1;
    }
}
