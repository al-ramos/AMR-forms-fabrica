using MediatR;
using RDS.Forms.Fabrica.Application.Features.Fichas.Commands;
using RDS.Forms.Fabrica.Domain.Interfaces;

namespace RDS.Forms.Fabrica.Application.Features.Fichas.Handlers;

public class RegistrarSaidaHandler(IFichaRepository repo, IUnitOfWork uow) : IRequestHandler<RegistrarSaidaCommand>
{
    public async Task Handle(RegistrarSaidaCommand request, CancellationToken ct)
    {
        var ficha = await repo.ObterPorIdAsync(request.FichaId)
            ?? throw new KeyNotFoundException($"Ficha {request.FichaId} não encontrada.");
        ficha.Finalizar();
        await repo.AtualizarAsync(ficha);
        await uow.SaveChangesAsync(ct);
    }
}
