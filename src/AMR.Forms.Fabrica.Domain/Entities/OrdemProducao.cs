namespace AMR.Forms.Fabrica.Domain.Entities;

public enum StatusOrdemProducao { Rascunho, Liberada, EmProducao, Concluida, Cancelada }

/// <summary>
/// Ordem de Produção (OP) com ciclo de vida completo e rastreabilidade integrada.
/// </summary>
public class OrdemProducao
{
    public int Id { get; private set; }
    public string Numero { get; private set; } = string.Empty;
    public int CodigoProduto { get; private set; }
    public int CodigoFilial { get; private set; }
    public decimal QuantidadePlanejada { get; private set; }
    public decimal QuantidadeProduzida { get; private set; }
    public decimal QuantidadeRejeitada { get; private set; }
    public StatusOrdemProducao Status { get; private set; }
    public DateTime DataAbertura { get; private set; }
    public DateTime? DataPrevistaFim { get; private set; }
    public DateTime? DataFechamento { get; private set; }
    public string? ObservacaoGeral { get; private set; }
    public string? MotivoCancelamento { get; private set; }

    // Navegação
    public Produto? Produto { get; private set; }
    public ICollection<RastreabilidadeItem> Rastreabilidade { get; private set; } = [];

    protected OrdemProducao() { }

    public OrdemProducao(string numero, int codigoProduto, int codigoFilial,
        decimal quantidadePlanejada, DateTime? dataPrevistaFim, string? observacao)
    {
        if (string.IsNullOrWhiteSpace(numero))
            throw new ArgumentException("Número da OP é obrigatório.", nameof(numero));
        if (codigoProduto <= 0)
            throw new ArgumentException("Código do produto deve ser positivo.", nameof(codigoProduto));
        if (quantidadePlanejada <= 0)
            throw new ArgumentException("Quantidade planejada deve ser positiva.", nameof(quantidadePlanejada));

        Numero = numero;
        CodigoProduto = codigoProduto;
        CodigoFilial = codigoFilial;
        QuantidadePlanejada = quantidadePlanejada;
        DataPrevistaFim = dataPrevistaFim;
        ObservacaoGeral = observacao;
        Status = StatusOrdemProducao.Rascunho;
        DataAbertura = DateTime.UtcNow;
    }

    public void Liberar()
    {
        if (Status != StatusOrdemProducao.Rascunho)
            throw new InvalidOperationException($"OP não pode ser liberada no status {Status}.");
        Status = StatusOrdemProducao.Liberada;
    }

    public void IniciarProducao()
    {
        if (Status != StatusOrdemProducao.Liberada)
            throw new InvalidOperationException($"OP deve estar Liberada para iniciar produção. Status atual: {Status}.");
        Status = StatusOrdemProducao.EmProducao;
    }

    public void RegistrarProducao(decimal quantidade)
    {
        if (Status != StatusOrdemProducao.EmProducao)
            throw new InvalidOperationException("OP deve estar Em Produção para registrar produção.");
        if (quantidade <= 0)
            throw new ArgumentException("Quantidade deve ser positiva.", nameof(quantidade));

        QuantidadeProduzida += quantidade;
    }

    public void RegistrarRejeicao(decimal quantidade)
    {
        if (Status != StatusOrdemProducao.EmProducao)
            throw new InvalidOperationException("OP deve estar Em Produção para registrar rejeição.");
        if (quantidade <= 0)
            throw new ArgumentException("Quantidade deve ser positiva.", nameof(quantidade));

        QuantidadeRejeitada += quantidade;
    }

    public void Concluir()
    {
        if (Status != StatusOrdemProducao.EmProducao)
            throw new InvalidOperationException("OP deve estar Em Produção para ser concluída.");

        Status = StatusOrdemProducao.Concluida;
        DataFechamento = DateTime.UtcNow;
    }

    public void Cancelar(string motivo)
    {
        if (Status == StatusOrdemProducao.Concluida || Status == StatusOrdemProducao.Cancelada)
            throw new InvalidOperationException($"OP no status {Status} não pode ser cancelada.");
        if (string.IsNullOrWhiteSpace(motivo))
            throw new ArgumentException("Motivo do cancelamento é obrigatório.", nameof(motivo));

        Status = StatusOrdemProducao.Cancelada;
        MotivoCancelamento = motivo;
        DataFechamento = DateTime.UtcNow;
    }

    public decimal QuantidadeRestante => QuantidadePlanejada - QuantidadeProduzida - QuantidadeRejeitada;
    public decimal PercentualConclusao => QuantidadePlanejada > 0
        ? Math.Round(QuantidadeProduzida / QuantidadePlanejada * 100, 1)
        : 0;
}
