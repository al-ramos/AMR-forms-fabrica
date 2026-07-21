namespace AMR.Forms.Fabrica.Application.Features.Bom.DTOs;

public record BomItemDto(
    int CodigoProdutoPai,
    int CodigoProdutoFilho,
    string? NomeProdutoFilho,
    string? CodigoProdutoLongo,
    string? UnidadeMedida,
    string? TipoProduto,
    decimal Quantidade,
    decimal PercentualPerda,
    decimal QuantidadeLiquida,
    int Nivel,
    bool Ativo,
    IReadOnlyList<BomItemDto> Componentes
);

public record ExplosaoMaterialDto(
    int CodigoProduto,
    string? NomeProduto,
    string? CodigoProdutoLongo,
    string? UnidadeMedida,
    string? TipoProduto,
    decimal QuantidadePorUnidadePai,
    decimal QuantidadeTotalAcumulada,
    decimal PercentualPerda,
    int Nivel
);

public record CustoBomDto(
    int CodigoProduto,
    string? NomeProduto,
    decimal CustoPadrao,
    decimal CustoTotal,
    IReadOnlyList<CustoBomItemDto> Componentes
);

public record CustoBomItemDto(
    int CodigoProduto,
    string? NomeProduto,
    string? TipoProduto,
    decimal Quantidade,
    decimal PercentualPerda,
    decimal CustoUnitario,
    decimal CustoExtendido,
    int Nivel
);
