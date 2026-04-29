using MediatR;
using RDS.Forms.Fabrica.Application.Features.Fichas.Queries;
using RDS.Forms.Fabrica.Domain.Entities;
using RDS.Forms.Fabrica.Domain.Interfaces;

namespace RDS.Forms.Fabrica.Application.Features.Fichas.Handlers;

public class GetFichasHandler(IFichaRepository repo) : IRequestHandler<GetFichasQuery, IEnumerable<Ficha>>
{
    public async Task<IEnumerable<Ficha>> Handle(GetFichasQuery request, CancellationToken ct)
        => request.DtInicio.HasValue && request.DtFim.HasValue
            ? await repo.ListarPorDataAsync(request.CdFilial, request.DtInicio.Value, request.DtFim.Value)
            : await repo.ListarPorFilialAsync(request.CdFilial);
}
