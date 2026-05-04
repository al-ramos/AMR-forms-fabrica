using MediatR;
using AMR.Forms.Fabrica.Application.Features.Filiais.Queries;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;

namespace AMR.Forms.Fabrica.Application.Features.Filiais.Handlers;

public class GetFiliaisHandler(IFilialRepository repo)
    : IRequestHandler<GetFiliaisQuery, IEnumerable<Filial>>
{
    public async Task<IEnumerable<Filial>> Handle(GetFiliaisQuery request, CancellationToken ct)
        => await repo.ListarTodosAsync();
}
