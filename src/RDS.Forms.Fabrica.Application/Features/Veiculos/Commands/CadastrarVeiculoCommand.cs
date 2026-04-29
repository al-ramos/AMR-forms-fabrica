using MediatR;

namespace RDS.Forms.Fabrica.Application.Features.Veiculos.Commands;

public record CadastrarVeiculoCommand(
    string Placa,
    int CodigoFilial,
    string? UfVeiculo,
    string? CodigoRntc) : IRequest<string>;
