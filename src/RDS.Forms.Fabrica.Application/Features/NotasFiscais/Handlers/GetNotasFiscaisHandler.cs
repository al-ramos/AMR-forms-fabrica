using MediatR;
using RDS.Forms.Fabrica.Application.Features.NotasFiscais.Queries;
using RDS.Forms.Fabrica.Domain.Entities;
using RDS.Forms.Fabrica.Domain.Interfaces;

namespace RDS.Forms.Fabrica.Application.Features.NotasFiscais.Handlers;

public class GetNotasFiscaisHandler(INotaFiscalRepository repo)
    : IRequestHandler<GetNotasFiscaisQuery, IEnumerable<NotaFiscal>>
{
    public async Task<IEnumerable<NotaFiscal>> Handle(GetNotasFiscaisQuery request, CancellationToken ct)
    {
        var inicio = request.DtInicio ?? DateOnly.FromDateTime(DateTime.Today.AddDays(-30));
        var fim = request.DtFim ?? DateOnly.FromDateTime(DateTime.Today);
        return await repo.ListarPorFilialEDataAsync(request.CdFilial, inicio, fim);
    }
}

public class GetNotaFiscalItensHandler(INotaFiscalDetalheRepository repo)
    : IRequestHandler<GetNotaFiscalItensQuery, IEnumerable<NotaFiscalDetalhe>>
{
    public async Task<IEnumerable<NotaFiscalDetalhe>> Handle(GetNotaFiscalItensQuery request, CancellationToken ct)
        => await repo.ListarPorNotaFiscalAsync(request.Numero, request.Serie);
}
