using MediatR;
using AMR.Forms.Fabrica.Domain.Entities;

namespace AMR.Forms.Fabrica.Application.Features.Filiais.Queries;

public record GetFiliaisQuery : IRequest<IEnumerable<Filial>>;
