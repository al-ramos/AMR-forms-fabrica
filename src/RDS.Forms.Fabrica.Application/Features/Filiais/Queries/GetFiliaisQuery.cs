using MediatR;
using RDS.Forms.Fabrica.Domain.Entities;

namespace RDS.Forms.Fabrica.Application.Features.Filiais.Queries;

public record GetFiliaisQuery : IRequest<IEnumerable<Filial>>;
