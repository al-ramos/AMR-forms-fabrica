namespace AMR.Forms.Fabrica.Domain.Entities;

public enum StatusOrdemManutencao { Pendente, EmExecucao, Concluida, Cancelada }

/// <summary>
/// Ordem de Manutenção (OM) — instância de execução de um plano ou solicitação avulsa.
/// Ciclo: Pendente → EmExecucao → Concluida | Cancelada
/// </summary>
public class OrdemManutencao
{
    public int Id { get; private set; }
    public int? PlanoManutencaoId { get; private set; }   // null = avulsa
    public int EquipamentoId { get; private set; }
    public int CodigoFilial { get; private set; }
    public TipoManutencao TipoManutencao { get; private set; }
    public string Descricao { get; private set; } = string.Empty;
    public StatusOrdemManutencao Status { get; private set; }
    public DateTime DataPrevista { get; private set; }
    public DateTime? DataInicio { get; private set; }
    public DateTime? DataConclusao { get; private set; }
    public decimal? DuracaoRealHoras { get; private set; }
    public string? CodigoTecnico { get; private set; }
    public string? Observacao { get; private set; }
    public string? MotivoCancelamento { get; private set; }
    public DateTime CriadoEm { get; private set; }

    // Navegação
    public Equipamento? Equipamento { get; private set; }
    public PlanoManutencao? PlanoManutencao { get; private set; }

    protected OrdemManutencao() { }

    public OrdemManutencao(
        int equipamentoId, int codigoFilial, TipoManutencao tipoManutencao,
        string descricao, DateTime dataPrevista, string? codigoTecnico,
        string? observacao, int? planoManutencaoId = null)
    {
        if (equipamentoId <= 0)
            throw new ArgumentException("ID do equipamento deve ser positivo.", nameof(equipamentoId));
        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("Descrição é obrigatória.", nameof(descricao));

        EquipamentoId      = equipamentoId;
        CodigoFilial       = codigoFilial;
        TipoManutencao     = tipoManutencao;
        Descricao          = descricao.Trim();
        DataPrevista       = dataPrevista;
        CodigoTecnico      = codigoTecnico;
        Observacao         = observacao;
        PlanoManutencaoId  = planoManutencaoId;
        Status             = StatusOrdemManutencao.Pendente;
        CriadoEm          = DateTime.UtcNow;
    }

    public void IniciarExecucao(string? codigoTecnico)
    {
        if (Status != StatusOrdemManutencao.Pendente)
            throw new InvalidOperationException($"OM não pode ser iniciada no status {Status}.");

        Status        = StatusOrdemManutencao.EmExecucao;
        DataInicio    = DateTime.UtcNow;
        CodigoTecnico = codigoTecnico ?? CodigoTecnico;
    }

    public void Concluir(decimal duracaoRealHoras, string? observacao)
    {
        if (Status != StatusOrdemManutencao.EmExecucao)
            throw new InvalidOperationException("OM deve estar Em Execução para ser concluída.");
        if (duracaoRealHoras <= 0)
            throw new ArgumentException("Duração real deve ser positiva.", nameof(duracaoRealHoras));

        Status            = StatusOrdemManutencao.Concluida;
        DataConclusao     = DateTime.UtcNow;
        DuracaoRealHoras  = duracaoRealHoras;
        if (observacao is not null) Observacao = observacao;
    }

    public void Cancelar(string motivo)
    {
        if (Status == StatusOrdemManutencao.Concluida || Status == StatusOrdemManutencao.Cancelada)
            throw new InvalidOperationException($"OM no status {Status} não pode ser cancelada.");
        if (string.IsNullOrWhiteSpace(motivo))
            throw new ArgumentException("Motivo é obrigatório.", nameof(motivo));

        Status               = StatusOrdemManutencao.Cancelada;
        MotivoCancelamento   = motivo;
        DataConclusao        = DateTime.UtcNow;
    }

    /// <summary>Atraso em dias em relação à data prevista (negativo = adiantado).</summary>
    public int? AtrasoEmDias => Status == StatusOrdemManutencao.Concluida && DataConclusao.HasValue
        ? (int)(DataConclusao.Value - DataPrevista).TotalDays
        : Status == StatusOrdemManutencao.Pendente || Status == StatusOrdemManutencao.EmExecucao
            ? (int)(DateTime.UtcNow - DataPrevista).TotalDays
            : null;
}
