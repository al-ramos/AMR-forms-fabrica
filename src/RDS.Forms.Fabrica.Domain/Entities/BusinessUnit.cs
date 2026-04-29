namespace RDS.Forms.Fabrica.Domain.Entities;

/// <summary>
/// Representa uma unidade de negócio (centro de custo / armazém) do JD Edwards.
/// </summary>
public class BusinessUnit
{
    public string Codigo { get; private set; } = null!;
    public string? Nome { get; private set; }
    public string? CodigoCompanhia { get; private set; }
    public int? CodigoAddressNumber { get; private set; }

    protected BusinessUnit() { }

    public BusinessUnit(string codigo, string? nome, string? codigoCompanhia, int? codigoAddressNumber)
    {
        if (string.IsNullOrWhiteSpace(codigo))
            throw new ArgumentException("Código da Business Unit é obrigatório.", nameof(codigo));

        Codigo = codigo;
        Nome = nome;
        CodigoCompanhia = codigoCompanhia;
        CodigoAddressNumber = codigoAddressNumber;
    }
}
