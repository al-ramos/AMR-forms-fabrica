using MediatR;
using AMR.Forms.Fabrica.Domain.Entities;

namespace AMR.Forms.Fabrica.Application.Features.Fichas.Queries;

public record GetFichasQuery(int CdFilial, DateOnly? DtInicio, DateOnly? DtFim)
    : IRequest<IEnumerable<Ficha>>;
