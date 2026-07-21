namespace AMR.Forms.Fabrica.Domain.Entities;

/// <summary>
/// Item da estrutura de BOM (Bill of Materials) com suporte a múltiplos níveis.
/// </summary>
public class BomItem
{
    public int Id { get; private set; }
    public int CodigoProdutoPai { get; private set; }
    public int CodigoProdutoFilho { get; private set; }
    public decimal Quantidade { get; private set; }
    public int Nivel { get; private set; }
    public decimal PercentualPerda { get; private set; }
    public bool Ativo { get; private set; }
    public DateTime CriadoEm { get; private set; }
    public DateTime? AtualizadoEm { get; private set; }

    // Navegação
    public Produto? ProdutoPai { get; private set; }
    public Produto? ProdutoFilho { get; private set; }

    protected BomItem() { }

    public BomItem(int codigoProdutoPai, int codigoProdutoFilho, decimal quantidade, int nivel, decimal percentualPerda = 0)
    {
        if (codigoProdutoPai <= 0)
            throw new ArgumentException("Código do produto pai deve ser positivo.", nameof(codigoProdutoPai));
        if (codigoProdutoFilho <= 0)
            throw new ArgumentException("Código do produto filho deve ser positivo.", nameof(codigoProdutoFilho));
        if (codigoProdutoPai == codigoProdutoFilho)
            throw new ArgumentException("Um produto não pode ser componente de si mesmo.");
        if (quantidade <= 0)
            throw new ArgumentException("Quantidade deve ser positiva.", nameof(quantidade));
        if (nivel < 1)
            throw new ArgumentException("Nível deve ser maior ou igual a 1.", nameof(nivel));
        if (percentualPerda < 0 || percentualPerda >= 100)
            throw new ArgumentException("Percentual de perda deve estar entre 0 e 99.99.", nameof(percentualPerda));

        CodigoProdutoPai = codigoProdutoPai;
        CodigoProdutoFilho = codigoProdutoFilho;
        Quantidade = quantidade;
        Nivel = nivel;
        PercentualPerda = percentualPerda;
        Ativo = true;
        CriadoEm = DateTime.UtcNow;
    }

    public void Desativar()
    {
        Ativo = false;
        AtualizadoEm = DateTime.UtcNow;
    }

    public void AtualizarQuantidade(decimal novaQuantidade, decimal novoPercentualPerda)
    {
        if (novaQuantidade <= 0)
            throw new ArgumentException("Quantidade deve ser positiva.", nameof(novaQuantidade));
        if (novoPercentualPerda < 0 || novoPercentualPerda >= 100)
            throw new ArgumentException("Percentual de perda deve estar entre 0 e 99.99.", nameof(novoPercentualPerda));

        Quantidade = novaQuantidade;
        PercentualPerda = novoPercentualPerda;
        AtualizadoEm = DateTime.UtcNow;
    }

    /// <summary>Quantidade considerando percentual de perda.</summary>
    public decimal QuantidadeLiquida => Quantidade * (1 + PercentualPerda / 100);
}
