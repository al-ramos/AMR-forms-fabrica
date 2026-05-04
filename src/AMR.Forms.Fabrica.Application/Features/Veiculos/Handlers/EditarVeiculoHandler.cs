using MediatR;
using AMR.Forms.Fabrica.Application.Features.Veiculos.Commands;
using AMR.Forms.Fabrica.Domain.Interfaces;

namespace AMR.Forms.Fabrica.Application.Features.Veiculos.Handlers;

public class EditarVeiculoHandler(IVeiculoRepository veiculoRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<EditarVeiculoCommand, string>
{
    public async Task<string> Handle(EditarVeiculoCommand request, CancellationToken ct)
    {
        var veiculo = await veiculoRepository.ObterPorPlacaAsync(request.Placa)
            ?? throw new KeyNotFoundException($"Veículo com placa '{request.Placa}' não encontrado.");

        veiculo.Atualizar(request.CodigoFilial, request.UfVeiculo, request.CodigoRntc);

        await veiculoRepository.AtualizarAsync(veiculo);
        await unitOfWork.SaveChangesAsync(ct);

        return veiculo.Placa;
    }
}
