using MediatR;
using AMR.Forms.Fabrica.Application.Features.Veiculos.Commands;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;

namespace AMR.Forms.Fabrica.Application.Features.Veiculos.Handlers;

public class CadastrarVeiculoHandler(IVeiculoRepository repo, IUnitOfWork uow)
    : IRequestHandler<CadastrarVeiculoCommand, string>
{
    public async Task<string> Handle(CadastrarVeiculoCommand request, CancellationToken ct)
    {
        var veiculo = new Veiculo(request.Placa, request.CodigoFilial, request.UfVeiculo, request.CodigoRntc);
        await repo.AdicionarAsync(veiculo);
        await uow.SaveChangesAsync(ct);
        return veiculo.Placa;
    }
}