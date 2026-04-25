using MediatR;
using RDS.Forms.Fabrica.Application.Features.Filiais.Queries;
using RDS.Forms.Fabrica.Domain.Entities;
using RDS.Forms.Fabrica.Domain.Interfaces;

namespace RDS.Forms.Fabrica.Application.Features.Filiais.Handlers;

public class GetFiliaisHandler(IFilialRepository repo)
    : IRequestHandler<GetFiliaisQuery, IEnumerable<Filial>>
{
    public async Task<IEnumerable<Filial>> Handle(GetFiliaisQuery request, CancellationToken ct)
        => await repo.ListarTodosAsync();
}
