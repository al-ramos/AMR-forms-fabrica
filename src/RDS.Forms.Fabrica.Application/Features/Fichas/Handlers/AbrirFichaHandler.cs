using MediatR;
using RDS.Forms.Fabrica.Application.Features.Fichas.Commands;
using RDS.Forms.Fabrica.Domain.Entities;
using RDS.Forms.Fabrica.Domain.Interfaces;

namespace RDS.Forms.Fabrica.Application.Features.Fichas.Handlers;

public class AbrirFichaHandler(IFichaRepository repo, IUnitOfWork uow) : IRequestHandler<AbrirFichaCommand, int>
{
    public async Task<int> Handle(AbrirFichaCommand request, CancellationToken ct)
    {
        var ficha = new Ficha(0, request.CodigoFilial, request.PlacaVeiculo, null, request.CodigoTipoOperacao, null, null);
        await repo.AdicionarAsync(ficha);
        await uow.SaveChangesAsync(ct);
        return ficha.Codigo;
    }
}
