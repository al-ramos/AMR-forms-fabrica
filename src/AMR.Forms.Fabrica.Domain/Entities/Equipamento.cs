namespace AMR.Forms.Fabrica.Domain.Entities;

/// <summary>
/// Equipamento de produção monitorado pelo OEE.
/// </summary>
public class Equipamento
{
    public int Id { get; private set; }
    public int CodigoFilial { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string? Descricao { get; private set; }
    public string? CodigoArea { get; private set; }
    public bool Ativo { get; private set; }
    public DateTime CriadoEm { get; private set; }

    // Navegação
    public ICollection<RegistroOee> RegistrosOee { get; private set; } = [];

    protected Equipamento() { }

    public Equipamento(int codigoFilial, string nome, string? descricao, string? codigoArea)
    {
        if (codigoFilial <= 0)
            throw new ArgumentException("Código da filial deve ser positivo.", nameof(codigoFilial));
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do equipamento é obrigatório.", nameof(nome));

        CodigoFilial = codigoFilial;
        Nome = nome.Trim();
        Descricao = descricao?.Trim();
        CodigoArea = codigoArea?.Trim();
        Ativo = true;
        CriadoEm = DateTime.UtcNow;
    }

    public void Atualizar(string nome, string? descricao, string? codigoArea)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do equipamento é obrigatório.", nameof(nome));

        Nome = nome.Trim();
        Descricao = descricao?.Trim();
        CodigoArea = codigoArea?.Trim();
    }

    public void Desativar() => Ativo = false;
    public void Ativar()    => Ativo = true;
}
