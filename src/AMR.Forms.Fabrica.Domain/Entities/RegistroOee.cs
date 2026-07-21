namespace AMR.Forms.Fabrica.Domain.Entities;

/// <summary>
/// Registro de OEE (Overall Equipment Effectiveness) de um turno/período de produção.
/// OEE = Disponibilidade × Performance × Qualidade
/// </summary>
public class RegistroOee
{
    public int Id { get; private set; }
    public int EquipamentoId { get; private set; }
    public int CodigoFilial { get; private set; }
    public DateTime DataHoraInicio { get; private set; }
    public DateTime DataHoraFim { get; private set; }

    /// <summary>Tempo planejado para produção, em minutos.</summary>
    public int TempoPlanejadoMinutos { get; private set; }

    /// <summary>Tempo real em que o equipamento produziu, em minutos (descontadas paradas).</summary>
    public int TempoRealProducaoMinutos { get; private set; }

    /// <summary>Total de peças produzidas (boas + ruins).</summary>
    public decimal QuantidadeProduzida { get; private set; }

    /// <summary>Peças aprovadas (sem defeito).</summary>
    public decimal QuantidadeAprovada { get; private set; }

    /// <summary>Tempo de ciclo ideal por peça, em segundos.</summary>
    public decimal TempoCicloIdealSegundos { get; private set; }

    public string? CodigoOperador { get; private set; }
    public string? Observacao { get; private set; }
    public DateTime CriadoEm { get; private set; }

    // Navegação
    public Equipamento? Equipamento { get; private set; }

    // ── Métricas OEE (computed) ───────────────────────────────────────────────

    /// <summary>Disponibilidade = TempoReal / TempoPlanejado (0–1).</summary>
    public decimal Disponibilidade =>
        TempoPlanejadoMinutos > 0
            ? Math.Min(1m, Math.Round((decimal)TempoRealProducaoMinutos / TempoPlanejadoMinutos, 4))
            : 0m;

    /// <summary>Performance = (Produzido × CicloIdeal) / (TempoReal em segundos) (0–1).</summary>
    public decimal Performance =>
        TempoRealProducaoMinutos > 0 && TempoCicloIdealSegundos > 0
            ? Math.Min(1m, Math.Round(
                QuantidadeProduzida * TempoCicloIdealSegundos / (TempoRealProducaoMinutos * 60m), 4))
            : 0m;

    /// <summary>Qualidade = Aprovado / Produzido (0–1).</summary>
    public decimal Qualidade =>
        QuantidadeProduzida > 0
            ? Math.Min(1m, Math.Round(QuantidadeAprovada / QuantidadeProduzida, 4))
            : 0m;

    /// <summary>OEE = Disponibilidade × Performance × Qualidade (0–1).</summary>
    public decimal Oee => Math.Round(Disponibilidade * Performance * Qualidade, 4);

    protected RegistroOee() { }

    public RegistroOee(
        int equipamentoId,
        int codigoFilial,
        DateTime dataHoraInicio,
        DateTime dataHoraFim,
        int tempoPlanejadoMinutos,
        int tempoRealProducaoMinutos,
        decimal quantidadeProduzida,
        decimal quantidadeAprovada,
        decimal tempoCicloIdealSegundos,
        string? codigoOperador,
        string? observacao)
    {
        if (equipamentoId <= 0)
            throw new ArgumentException("ID do equipamento deve ser positivo.", nameof(equipamentoId));
        if (dataHoraFim <= dataHoraInicio)
            throw new ArgumentException("Data/hora fim deve ser posterior ao início.");
        if (tempoPlanejadoMinutos <= 0)
            throw new ArgumentException("Tempo planejado deve ser positivo.", nameof(tempoPlanejadoMinutos));
        if (tempoRealProducaoMinutos < 0)
            throw new ArgumentException("Tempo real não pode ser negativo.", nameof(tempoRealProducaoMinutos));
        if (quantidadeProduzida < 0)
            throw new ArgumentException("Quantidade produzida não pode ser negativa.", nameof(quantidadeProduzida));
        if (quantidadeAprovada < 0 || quantidadeAprovada > quantidadeProduzida)
            throw new ArgumentException("Quantidade aprovada inválida.", nameof(quantidadeAprovada));
        if (tempoCicloIdealSegundos <= 0)
            throw new ArgumentException("Tempo de ciclo ideal deve ser positivo.", nameof(tempoCicloIdealSegundos));

        EquipamentoId = equipamentoId;
        CodigoFilial = codigoFilial;
        DataHoraInicio = dataHoraInicio;
        DataHoraFim = dataHoraFim;
        TempoPlanejadoMinutos = tempoPlanejadoMinutos;
        TempoRealProducaoMinutos = tempoRealProducaoMinutos;
        QuantidadeProduzida = quantidadeProduzida;
        QuantidadeAprovada = quantidadeAprovada;
        TempoCicloIdealSegundos = tempoCicloIdealSegundos;
        CodigoOperador = codigoOperador;
        Observacao = observacao;
        CriadoEm = DateTime.UtcNow;
    }
}
