using AMR.Forms.Fabrica.Domain.Entities;

namespace AMR.Forms.Fabrica.Application.Features.OrdensProducao.DTOs;

public record OrdemProducaoDto(
    int Id,
    string Numero,
    int CodigoProduto,
    string? NomeProduto,
    int CodigoFilial,
    decimal QuantidadePlanejada,
    decimal QuantidadeProduzida,
    decimal QuantidadeRejeitada,
    decimal QuantidadeRestante,
    decimal PercentualConclusao,
    StatusOrdemProducao Status,
    DateTime DataAbertura,
    DateTime? DataPrevistaFim,
    DateTime? DataFechamento,
    string? ObservacaoGeral,
    string? MotivoCancelamento
);

public record RastreabilidadeItemDto(
    int Id,
    int OrdemProducaoId,
    string NumeroOp,
    int CodigoProduto,
    string? NomeProduto,
    string? Lote,
    decimal Quantidade,
    TipoMovimentoRastreabilidade TipoMovimento,
    DateTime DataHoraRegistro,
    string? CodigoOperador,
    string? Observacao
);
