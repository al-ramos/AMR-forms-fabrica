using MediatR;

namespace AMR.Forms.Fabrica.Application.Features.Veiculos.Commands;

public record EditarVeiculoCommand(
    string Placa,
    int CodigoFilial,
    string? UfVeiculo,
    string? CodigoRntc) : IRequest<string>;
