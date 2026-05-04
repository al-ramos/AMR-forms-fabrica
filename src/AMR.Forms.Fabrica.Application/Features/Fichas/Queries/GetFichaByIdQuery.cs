using MediatR;
using AMR.Forms.Fabrica.Domain.Entities;

namespace AMR.Forms.Fabrica.Application.Features.Fichas.Queries;

public record GetFichaByIdQuery(int Id) : IRequest<Ficha?>;
