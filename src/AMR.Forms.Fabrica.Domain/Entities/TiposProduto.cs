namespace AMR.Forms.Fabrica.Domain.Entities;

/// <summary>
/// Constantes para o campo <see cref="Produto.TipoProduto"/>.
/// Evita magic-strings espalhadas em handlers e repositórios.
/// </summary>
public static class TiposProduto
{
    public const string Fabricado = "Fabricado";
    public const string Comprado  = "Comprado";
    public const string Fantasma  = "Fantasma";
}
