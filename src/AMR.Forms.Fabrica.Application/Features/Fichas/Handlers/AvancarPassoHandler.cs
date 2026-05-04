using MediatR;
using AMR.Forms.Fabrica.Application.Features.Fichas.Commands;
using AMR.Forms.Fabrica.Domain.Interfaces;

namespace AMR.Forms.Fabrica.Application.Features.Fichas.Handlers;

public class AvancarPassoHandler(IFichaRepository repo, IUnitOfWork uow) : IRequestHandler<AvancarPassoCommand>
{
    public async Task Handle(AvancarPassoCommand request, CancellationToken ct)
    {
        var ficha = await repo.ObterPorIdAsync(request.FichaId)
            ?? throw new KeyNotFoundException($"Ficha {request.FichaId} não encontrada.");
        ficha.AvancarPasso(request.ProximoPasso);
        await repo.AtualizarAsync(ficha);
        await uow.SaveChangesAsync(ct);
    }
}
