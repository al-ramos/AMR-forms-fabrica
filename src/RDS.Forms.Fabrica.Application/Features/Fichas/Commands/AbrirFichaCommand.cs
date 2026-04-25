using MediatR;

namespace RDS.Forms.Fabrica.Application.Features.Fichas.Commands;

public record AbrirFichaCommand(int CodigoFilial, string PlacaVeiculo, int CodigoTipoOperacao)
    : IRequest<int>;
