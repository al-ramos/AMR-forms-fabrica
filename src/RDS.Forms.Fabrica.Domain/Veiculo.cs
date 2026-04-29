namespace RDS.Forms.Fabrica.Domain.Entities;

/// <summary>
/// Representa um veículo que realiza operações de carga/descarga na fábrica.
/// </summary>
public class Veiculo
{
    public string Placa { get; private set; } = null!;
    public int CodigoFilial { get; private set; }
    public string? UfVeiculo { get; private set; }
    public string? CodigoRntc { get; private set; }

    protected Veiculo() { }

    public Veiculo(string placa, int codigoFilial, string? ufVeiculo, string? codigoRntc)
    {
        if (string.IsNullOrWhiteSpace(placa))
            throw new ArgumentException("Placa do veículo é obrigatória.", nameof(placa));

        Placa = placa.ToUpper();
        CodigoFilial = codigoFilial;
        UfVeiculo = ufVeiculo;
        CodigoRntc = codigoRntc;
    }

    public void Atualizar(int codigoFilial, string? ufVeiculo, string? codigoRntc)
    {
        CodigoFilial = codigoFilial;
        UfVeiculo = ufVeiculo;
        CodigoRntc = codigoRntc;
    }
}
