namespace AMR.Forms.Fabrica.Domain.Entities;

public enum TipoManutencao { Preventiva, Preditiva, Corretiva }

/// <summary>
/// Plano recorrente de manutenção para um equipamento.
/// Define periodicidade, tipo e duração estimada.
/// </summary>
public class PlanoManutencao
{
    public int Id { get; private set; }
    public int EquipamentoId { get; private set; }
    public int CodigoFilial { get; private set; }
    public TipoManutencao TipoManutencao { get; private set; }
    public string Descricao { get; private set; } = string.Empty;
    public string? Instrucoes { get; private set; }

    /// <summary>Periodicidade em dias (ex: 30 = mensal).</summary>
    public int FrequenciaDias { get; private set; }

    /// <summary>Duração estimada para execução, em horas.</summary>
    public decimal DuracaoEstimadaHoras { get; private set; }

    public DateTime ProximaExecucao { get; private set; }
    public DateTime? UltimaExecucao { get; private set; }
    public bool Ativo { get; private set; }
    public DateTime CriadoEm { get; private set; }

    // Navegação
    public Equipamento? Equipamento { get; private set; }
    public ICollection<OrdemManutencao> OrdensManutencao { get; private set; } = [];

    protected PlanoManutencao() { }

    public PlanoManutencao(
        int equipamentoId, int codigoFilial, TipoManutencao tipoManutencao,
        string descricao, string? instrucoes, int frequenciaDias,
        decimal duracaoEstimadaHoras, DateTime proximaExecucao)
    {
        if (equipamentoId <= 0)
            throw new ArgumentException("ID do equipamento deve ser positivo.", nameof(equipamentoId));
        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("Descrição é obrigatória.", nameof(descricao));
        if (frequenciaDias <= 0)
            throw new ArgumentException("Frequência em dias deve ser positiva.", nameof(frequenciaDias));
        if (duracaoEstimadaHoras <= 0)
            throw new ArgumentException("Duração estimada deve ser positiva.", nameof(duracaoEstimadaHoras));

        EquipamentoId      = equipamentoId;
        CodigoFilial       = codigoFilial;
        TipoManutencao     = tipoManutencao;
        Descricao          = descricao.Trim();
        Instrucoes         = instrucoes?.Trim();
        FrequenciaDias     = frequenciaDias;
        DuracaoEstimadaHoras = duracaoEstimadaHoras;
        ProximaExecucao    = proximaExecucao;
        Ativo              = true;
        CriadoEm          = DateTime.UtcNow;
    }

    public void Atualizar(string descricao, string? instrucoes, int frequenciaDias, decimal duracaoEstimadaHoras)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("Descrição é obrigatória.", nameof(descricao));
        if (frequenciaDias <= 0)
            throw new ArgumentException("Frequência em dias deve ser positiva.", nameof(frequenciaDias));
        if (duracaoEstimadaHoras <= 0)
            throw new ArgumentException("Duração estimada deve ser positiva.", nameof(duracaoEstimadaHoras));

        Descricao            = descricao.Trim();
        Instrucoes           = instrucoes?.Trim();
        FrequenciaDias       = frequenciaDias;
        DuracaoEstimadaHoras = duracaoEstimadaHoras;
    }

    /// <summary>Registra execução e avança ProximaExecucao pela frequência definida.</summary>
    public void RegistrarExecucao(DateTime dataExecucao)
    {
        UltimaExecucao  = dataExecucao;
        ProximaExecucao = dataExecucao.AddDays(FrequenciaDias);
    }

    public void Desativar() => Ativo = false;
    public void Ativar()    => Ativo = true;
}
