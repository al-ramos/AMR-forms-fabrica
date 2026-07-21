namespace AMR.Forms.Fabrica.Domain.Entities;

public enum TipoMovimentoRastreabilidade { Consumo, Producao, Rejeicao }

/// <summary>
/// Registro de rastreabilidade por lote de um item consumido ou produzido na OP.
/// </summary>
public class RastreabilidadeItem
{
    public int Id { get; private set; }
    public int OrdemProducaoId { get; private set; }
    public int CodigoProduto { get; private set; }
    public string? Lote { get; private set; }
    public decimal Quantidade { get; private set; }
    public TipoMovimentoRastreabilidade TipoMovimento { get; private set; }
    public DateTime DataHoraRegistro { get; private set; }
    public string? CodigoOperador { get; private set; }
    public string? Observacao { get; private set; }

    // Navegação
    public OrdemProducao? OrdemProducao { get; private set; }
    public Produto? Produto { get; private set; }

    protected RastreabilidadeItem() { }

    public RastreabilidadeItem(int ordemProducaoId, int codigoProduto, string? lote,
        decimal quantidade, TipoMovimentoRastreabilidade tipoMovimento,
        string? codigoOperador, string? observacao)
    {
        if (ordemProducaoId <= 0)
            throw new ArgumentException("ID da OP deve ser positivo.", nameof(ordemProducaoId));
        if (codigoProduto <= 0)
            throw new ArgumentException("Código do produto deve ser positivo.", nameof(codigoProduto));
        if (quantidade <= 0)
            throw new ArgumentException("Quantidade deve ser positiva.", nameof(quantidade));

        OrdemProducaoId = ordemProducaoId;
        CodigoProduto = codigoProduto;
        Lote = lote;
        Quantidade = quantidade;
        TipoMovimento = tipoMovimento;
        DataHoraRegistro = DateTime.UtcNow;
        CodigoOperador = codigoOperador;
        Observacao = observacao;
    }
}
