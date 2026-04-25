using MediatR;
using RDS.Forms.Fabrica.Application.Features.Fichas.Queries;
using RDS.Forms.Fabrica.Domain.Entities;
using RDS.Forms.Fabrica.Domain.Interfaces;

namespace RDS.Forms.Fabrica.Application.Features.Fichas.Handlers;

public class GetFichaByIdHandler(IFichaRepository repo) : IRequestHandler<GetFichaByIdQuery, Ficha?>
{
    public async Task<Ficha?> Handle(GetFichaByIdQuery request, CancellationToken ct)
        => await repo.ObterPorIdAsync(request.Id);
}
