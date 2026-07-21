namespace AMR.Forms.Fabrica.Domain.Entities;

/// <summary>
/// Representa um produto que transita pelas operações da fábrica.
/// </summary>
public class Produto
{
    public int Codigo { get; private set; }
    public string? CodigoBusinessUnit { get; private set; }
    public string? CodigoProdutoLongo { get; private set; }
    public string? Nome { get; private set; }
    public string? CodigoEan { get; private set; }
    public string? UnidadeMedidaComercial { get; private set; }
    public string? UnidadeMedida { get; private set; }
    public string? CodigoCtf { get; private set; }
    public string? CodigoClf { get; private set; }

    // ── Campos BOM (Sprint 24) ──────────────────────────────────────────
    /// <summary>Tipo do produto: Fabricado, Comprado ou Fantasma.</summary>
    public string? TipoProduto { get; private set; }
    /// <summary>Lead time em dias para aquisição ou produção.</summary>
    public int LeadTimeDias { get; private set; }
    /// <summary>Custo padrão unitário (moeda local).</summary>
    public decimal CustoPadrao { get; private set; }

    // Navegação BOM
    public ICollection<BomItem> BomComoFabricado { get; private set; } = [];
    public ICollection<BomItem> BomComoComponente { get; private set; } = [];

    protected Produto() { }

    public Produto(int codigo, string? nome, string? codigoProdutoLongo, string? codigoEan, string? unidadeMedida)
    {
        if (codigo <= 0)
            throw new ArgumentException("Código do produto deve ser positivo.", nameof(codigo));

        Codigo = codigo;
        Nome = nome;
        CodigoProdutoLongo = codigoProdutoLongo;
        CodigoEan = codigoEan;
        UnidadeMedida = unidadeMedida;
    }

    public void AtualizarDadosBom(string? tipoProduto, int leadTimeDias, decimal custoPadrao)
    {
        TipoProduto = tipoProduto;
        LeadTimeDias = leadTimeDias >= 0 ? leadTimeDias : throw new ArgumentException("Lead time não pode ser negativo.");
        CustoPadrao = custoPadrao >= 0 ? custoPadrao : throw new ArgumentException("Custo padrão não pode ser negativo.");
    }
}
