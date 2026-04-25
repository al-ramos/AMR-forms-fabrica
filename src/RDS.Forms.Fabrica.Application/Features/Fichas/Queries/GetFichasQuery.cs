using MediatR;
using RDS.Forms.Fabrica.Domain.Entities;

namespace RDS.Forms.Fabrica.Application.Features.Fichas.Queries;

public record GetFichasQuery(int CdFilial, DateOnly? DtInicio, DateOnly? DtFim)
    : IRequest<IEnumerable<Ficha>>;
