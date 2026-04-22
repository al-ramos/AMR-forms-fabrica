namespace RDS.Forms.Fabrica.Domain.Entities;

/// <summary>
/// Registra as pesagens (tara e bruto) realizadas em uma ficha de operação.
/// </summary>
public class FichaBalanca
{
    public int CodigoFicha { get; private set; }
    public int CodigoFilial { get; private set; }
    public int? CodigoPesagem { get; private set; }
    public string? OrigemDestino { get; private set; }
    public decimal? Peso1Pesagem { get; private set; }
    public decimal? Peso2Pesagem { get; private set; }

    public decimal? PesoLiquido => Peso1Pesagem.HasValue && Peso2Pesagem.HasValue
        ? Math.Abs(Peso1Pesagem.Value - Peso2Pesagem.Value)
        : null;

    protected FichaBalanca() { }

    public FichaBalanca(int codigoFicha, int codigoFilial, int? codigoPesagem, string? origemDestino, decimal? peso1, decimal? peso2)
    {
        CodigoFicha = codigoFicha;
        CodigoFilial = codigoFilial;
        CodigoPesagem = codigoPesagem;
        OrigemDestino = origemDestino;
        Peso1Pesagem = peso1;
        Peso2Pesagem = peso2;
    }
}
