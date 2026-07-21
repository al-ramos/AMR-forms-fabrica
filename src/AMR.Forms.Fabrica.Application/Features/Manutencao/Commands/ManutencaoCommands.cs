using AMR.Forms.Fabrica.Application.Common;
using AMR.Forms.Fabrica.Domain.Entities;
using MediatR;

namespace AMR.Forms.Fabrica.Application.Features.Manutencao.Commands;

public record CriarPlanoManutencaoCommand(
    int EquipamentoId,
    int CodigoFilial,
    TipoManutencao TipoManutencao,
    string Descricao,
    string? Instrucoes,
    int FrequenciaDias,
    decimal DuracaoEstimadaHoras,
    DateTime ProximaExecucao
) : IRequest<Result<int>>;

public record AtualizarPlanoManutencaoCommand(
    int Id,
    string Descricao,
    string? Instrucoes,
    int FrequenciaDias,
    decimal DuracaoEstimadaHoras
) : IRequest<Result>;

public record AlterarStatusPlanoCommand(int Id, bool Ativo) : IRequest<Result>;

/// <summary>Abre uma OM avulsa (sem plano) ou a partir de um plano.</summary>
public record AbrirOrdemManutencaoCommand(
    int EquipamentoId,
    int CodigoFilial,
    TipoManutencao TipoManutencao,
    string Descricao,
    DateTime DataPrevista,
    string? CodigoTecnico,
    string? Observacao,
    int? PlanoManutencaoId = null
) : IRequest<Result<int>>;

public record IniciarOrdemManutencaoCommand(int Id, string? CodigoTecnico) : IRequest<Result>;

public record ConcluirOrdemManutencaoCommand(int Id, decimal DuracaoRealHoras, string? Observacao) : IRequest<Result>;

public record CancelarOrdemManutencaoCommand(int Id, string Motivo) : IRequest<Result>;
