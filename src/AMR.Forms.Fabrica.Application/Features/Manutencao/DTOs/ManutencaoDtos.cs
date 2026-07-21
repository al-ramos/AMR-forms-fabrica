using AMR.Forms.Fabrica.Domain.Entities;

namespace AMR.Forms.Fabrica.Application.Features.Manutencao.DTOs;

public record PlanoManutencaoDto(
    int Id,
    int EquipamentoId,
    string? NomeEquipamento,
    int CodigoFilial,
    TipoManutencao TipoManutencao,
    string Descricao,
    string? Instrucoes,
    int FrequenciaDias,
    decimal DuracaoEstimadaHoras,
    DateTime ProximaExecucao,
    DateTime? UltimaExecucao,
    bool Ativo,
    DateTime CriadoEm
);

public record OrdemManutencaoDto(
    int Id,
    int? PlanoManutencaoId,
    int EquipamentoId,
    string? NomeEquipamento,
    int CodigoFilial,
    TipoManutencao TipoManutencao,
    string Descricao,
    StatusOrdemManutencao Status,
    DateTime DataPrevista,
    DateTime? DataInicio,
    DateTime? DataConclusao,
    decimal? DuracaoRealHoras,
    int? AtrasoEmDias,
    string? CodigoTecnico,
    string? Observacao,
    string? MotivoCancelamento,
    DateTime CriadoEm
);
