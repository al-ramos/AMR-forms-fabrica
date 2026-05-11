using MediatR;
using AMR.Forms.Fabrica.Application.Features.Fichas.Commands;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace AMR.Forms.Fabrica.Application.Features.Fichas.Handlers;

public class RegistrarSaidaHandler(
    IFichaRepository repo,
    IFichaBalancaRepository balancaRepo,
    IFinanceiroHttpClient financeiro,
    IUnitOfWork uow,
    ILogger<RegistrarSaidaHandler> logger)
    : IRequestHandler<RegistrarSaidaCommand>
{
    public async Task Handle(RegistrarSaidaCommand request, CancellationToken ct)
    {
        var ficha = await repo.ObterPorIdAsync(request.FichaId)
            ?? throw new KeyNotFoundException($"Ficha {request.FichaId} não encontrada.");

        ficha.Finalizar();
        await repo.AtualizarAsync(ficha);
        await uow.SaveChangesAsync(ct);

        // ── Integração AMR.Financeiro ─────────────────────────────────────────
        // Cria ContaPagar com base no peso líquido registrado na balança
        await CriarContaPagarSeAplicavel(ficha, ct);
    }

    private async Task CriarContaPagarSeAplicavel(
        Ficha ficha, CancellationToken ct)
    {
        var pesagens = await balancaRepo.ListarPorFichaAsync(ficha.Codigo);
        var balanca  = pesagens.FirstOrDefault();

        if (balanca?.PesoLiquido is not decimal peso || peso <= 0)
        {
            logger.LogInformation(
                "Ficha {Codigo}: sem pesagem registrada, ContaPagar não criada.", ficha.Codigo);
            return;
        }

        var placa     = ficha.PlacaVeiculo ?? "s/placa";
        var descricao = $"Fábrica - Saída Ficha #{ficha.Codigo} | {placa} | {peso:N0} kg";
        var hoje      = DateOnly.FromDateTime(DateTime.Today);

        var id = await financeiro.CriarContaPagarAsync(new(
            CdFilial:   ficha.CodigoFilial,
            Descricao:  descricao,
            Valor:      peso,               // peso líquido em kg como valor base
            Vencimento: hoje
        ), ct);

        if (id.HasValue)
            logger.LogInformation(
                "Ficha {Codigo}: ContaPagar #{Id} criada no Financeiro ({Valor:N0} kg).",
                ficha.Codigo, id.Value, peso);
        else
            logger.LogWarning(
                "Ficha {Codigo}: falha ao criar ContaPagar no Financeiro.", ficha.Codigo);
    }
}
