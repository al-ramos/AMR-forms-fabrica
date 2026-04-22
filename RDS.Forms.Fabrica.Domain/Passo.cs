namespace RDS.Forms.Fabrica.Domain.Entities;

/// <summary>
/// Representa um passo/etapa dentro do fluxo de operação de uma ficha de fábrica.
/// </summary>
public class Passo
{
    public int Codigo { get; private set; }
    public string? Nome { get; private set; }

    protected Passo() { }

    public Passo(int codigo, string? nome)
    {
        Codigo = codigo;
        Nome = nome;
    }
}
