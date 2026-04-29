using MediatR;
using RDS.Forms.Fabrica.Domain.Entities;

namespace RDS.Forms.Fabrica.Application.Features.Fichas.Queries;

public record GetFichaByIdQuery(int Id) : IRequest<Ficha?>;
