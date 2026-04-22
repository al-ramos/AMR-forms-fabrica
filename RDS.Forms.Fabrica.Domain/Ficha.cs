namespace RDS.Forms.Fabrica.Domain.Entities;

/// <summary>
/// Agregado raiz principal do sistema. Representa o checklist/ficha de controle
/// de uma operação de carga ou descarga na fábrica.
/// </summary>
public class Ficha
{
    public int Codigo { get; private set; }
    public int CodigoFilial { get; private set; }
    public string? PlacaVeiculo { get; private set; }
    public string? CodigoBusinessUnit { get; private set; }
    public int? CodigoTipoOperacao { get; private set; }
    public int? CodigoPassoAtual { get; private set; }
    public string? CodigoLotId { get; private set; }
    public DateOnly? DataFicha { get; private set; }
    public DateOnly? DataSaida { get; private set; }
    public DateOnly? DataInterfaceJde { get; private set; }
    public string? NomeMotorista { get; private set; }
    public string? CodigoContratoManifesto { get; private set; }
    public int? CodigoTransportadora { get; private set; }
    public int? CodigoProdutoDepto { get; private set; }
    public string? CodigoSolicitacaoTransp { get; private set; }
    public string? CodigoTipoDoctoJde { get; private set; }

    public bool EstaFinalizada => DataSaida.HasValue;
    public bool IntegradaComJde => DataInterfaceJde.HasValue;

    protected Ficha() { }

    public Ficha(
        int codigo,
        int codigoFilial,
        string? placaVeiculo,
        string? codigoBusinessUnit,
        int? codigoTipoOperacao,
        string? nomeMotorista,
        DateOnly? dataFicha)
    {
        if (codigo <= 0)
            throw new ArgumentException("Código da ficha deve ser positivo.", nameof(codigo));

        Codigo = codigo;
        CodigoFilial = codigoFilial;
        PlacaVeiculo = placaVeiculo;
        CodigoBusinessUnit = codigoBusinessUnit;
        CodigoTipoOperacao = codigoTipoOperacao;
        NomeMotorista = nomeMotorista;
        DataFicha = dataFicha ?? DateOnly.FromDateTime(DateTime.Today);
    }

    public void AvancarPasso(int codigoProximoPasso)
    {
        CodigoPassoAtual = codigoProximoPasso;
    }

    public void Finalizar()
    {
        if (EstaFinalizada)
            throw new InvalidOperationException("Ficha já está finalizada.");

        DataSaida = DateOnly.FromDateTime(DateTime.Today);
    }

    public void MarcarIntegracaoJde()
    {
        DataInterfaceJde = DateOnly.FromDateTime(DateTime.Today);
    }
}
