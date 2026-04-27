using MediatR;
using RDS.Forms.Fabrica.Domain.Entities;

namespace RDS.Forms.Fabrica.Application.Features.Veiculos.Queries;

public record GetVeiculosQuery() : IRequest<IEnumerable<Veiculo>>;
public record GetVeiculosPorFilialQuery(int CdFilial) : IRequest<IEnumerable<Veiculo>>;
