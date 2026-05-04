using MediatR;
using AMR.Forms.Fabrica.Application.Features.Veiculos.Queries;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;

namespace AMR.Forms.Fabrica.Application.Features.Veiculos.Handlers;

public class GetVeiculosHandler(IVeiculoRepository repo)
    : IRequestHandler<GetVeiculosQuery, IEnumerable<Veiculo>>
{
    public async Task<IEnumerable<Veiculo>> Handle(GetVeiculosQuery request, CancellationToken ct)
        => await repo.ListarTodosAsync();
}

public class GetVeiculosPorFilialHandler(IVeiculoRepository repo)
    : IRequestHandler<GetVeiculosPorFilialQuery, IEnumerable<Veiculo>>
{
    public async Task<IEnumerable<Veiculo>> Handle(GetVeiculosPorFilialQuery request, CancellationToken ct)
        => await repo.ListarPorFilialAsync(request.CdFilial);
}
