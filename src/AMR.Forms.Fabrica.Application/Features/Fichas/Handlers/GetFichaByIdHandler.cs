using MediatR;
using AMR.Forms.Fabrica.Application.Features.Fichas.Queries;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;

namespace AMR.Forms.Fabrica.Application.Features.Fichas.Handlers;

public class GetFichaByIdHandler(IFichaRepository repo) : IRequestHandler<GetFichaByIdQuery, Ficha?>
{
    public async Task<Ficha?> Handle(GetFichaByIdQuery request, CancellationToken ct)
        => await repo.ObterPorIdAsync(request.Id);
}
