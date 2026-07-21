using AMR.Forms.Fabrica.Application.Common;
using AMR.Forms.Fabrica.Domain.Entities;
using MediatR;

namespace AMR.Forms.Fabrica.Application.Features.OrdensProducao.Commands;

public record AbrirOrdemProducaoCommand(
    string Numero,
    int CodigoProduto,
    int CodigoFilial,
    decimal QuantidadePlanejada,
    DateTime? DataPrevistaFim,
    string? Observacao
) : IRequest<Result<int>>;

public record LiberarOpCommand(int Id) : IRequest<Result>;
public record IniciarProducaoCommand(int Id) : IRequest<Result>;

public record RegistrarProducaoCommand(
    int OrdemProducaoId,
    decimal Quantidade,
    string? Lote,
    string? CodigoOperador,
    string? Observacao
) : IRequest<Result>;

public record RegistrarRejeicaoCommand(
    int OrdemProducaoId,
    decimal Quantidade,
    string? Lote,
    string? CodigoOperador,
    string? MotivoRejeicao
) : IRequest<Result>;

public record ConcluirOpCommand(int Id) : IRequest<Result>;

public record CancelarOpCommand(int Id, string Motivo) : IRequest<Result>;
