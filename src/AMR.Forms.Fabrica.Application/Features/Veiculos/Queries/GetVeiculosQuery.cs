using MediatR;
using AMR.Forms.Fabrica.Domain.Entities;

namespace AMR.Forms.Fabrica.Application.Features.Veiculos.Queries;

public record GetVeiculosQuery() : IRequest<IEnumerable<Veiculo>>;
public record GetVeiculosPorFilialQuery(int CdFilial) : IRequest<IEnumerable<Veiculo>>;
