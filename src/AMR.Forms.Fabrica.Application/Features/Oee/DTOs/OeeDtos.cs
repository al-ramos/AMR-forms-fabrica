namespace AMR.Forms.Fabrica.Application.Features.Oee.DTOs;

public record EquipamentoDto(
    int Id,
    int CodigoFilial,
    string Nome,
    string? Descricao,
    string? CodigoArea,
    bool Ativo,
    DateTime CriadoEm
);

public record RegistroOeeDto(
    int Id,
    int EquipamentoId,
    string? NomeEquipamento,
    int CodigoFilial,
    DateTime DataHoraInicio,
    DateTime DataHoraFim,
    int TempoPlanejadoMinutos,
    int TempoRealProducaoMinutos,
    decimal QuantidadeProduzida,
    decimal QuantidadeAprovada,
    decimal TempoCicloIdealSegundos,
    decimal Disponibilidade,
    decimal Performance,
    decimal Qualidade,
    decimal Oee,
    string? CodigoOperador,
    string? Observacao,
    DateTime CriadoEm
);

/// <summary>Resumo de OEE médio de um equipamento num período.</summary>
public record OeeResumoDto(
    int EquipamentoId,
    string? NomeEquipamento,
    DateTime DataInicio,
    DateTime DataFim,
    int TotalRegistros,
    decimal DisponibilidadeMedia,
    decimal PerformanceMedia,
    decimal QualidadeMedia,
    decimal OeeMedia
);
