using AMR.Forms.Fabrica.Application.Common;
using MediatR;

namespace AMR.Forms.Fabrica.Application.Features.Oee.Commands;

/// <summary>Cadastra um novo equipamento.</summary>
public record CadastrarEquipamentoCommand(
    int CodigoFilial,
    string Nome,
    string? Descricao,
    string? CodigoArea
) : IRequest<Result<int>>;

/// <summary>Atualiza dados de um equipamento.</summary>
public record AtualizarEquipamentoCommand(
    int Id,
    string Nome,
    string? Descricao,
    string? CodigoArea
) : IRequest<Result>;

/// <summary>Ativa ou desativa um equipamento.</summary>
public record AlterarStatusEquipamentoCommand(int Id, bool Ativo) : IRequest<Result>;

/// <summary>Registra um período de OEE de um equipamento.</summary>
public record RegistrarOeeCommand(
    int EquipamentoId,
    int CodigoFilial,
    DateTime DataHoraInicio,
    DateTime DataHoraFim,
    int TempoPlanejadoMinutos,
    int TempoRealProducaoMinutos,
    decimal QuantidadeProduzida,
    decimal QuantidadeAprovada,
    decimal TempoCicloIdealSegundos,
    string? CodigoOperador,
    string? Observacao
) : IRequest<Result<int>>;
