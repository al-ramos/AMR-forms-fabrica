using MediatR;
using AMR.Forms.Fabrica.Application.Features.NotasFiscais.Commands;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace AMR.Forms.Fabrica.Application.Features.NotasFiscais.Handlers;

public class RegistrarTransmissaoNfHandler(
    INotaFiscalRepository nfRepo,
    IFinanceiroHttpClient financeiro,
    IUnitOfWork uow,
    ILogger<RegistrarTransmissaoNfHandler> logger)
    : IRequestHandler<RegistrarTransmissaoNfCommand>
{
    public async Task Handle(RegistrarTransmissaoNfCommand request, CancellationToken ct)
    {
        var nf = await nfRepo.ObterPorChaveAsync(request.Numero, request.Serie)
            ?? throw new KeyNotFoundException($"NF {request.Numero}/{request.Serie} não encontrada.");

        nf.RegistrarTransmissao(request.ChaveNfe, request.Protocolo, request.ValorTransmissao);
        await nfRepo.AtualizarAsync(nf);
        await uow.SaveChangesAsync(ct);

        // ── Integração AMR.Financeiro ─────────────────────────────────────────
        // NF transmitida com valor → gera ContaReceber (receita da operação)
        if (request.ValorTransmissao is decimal valor && valor > 0)
            await CriarContaReceber(nf, valor, ct);
    }

    private async Task CriarContaReceber(
        NotaFiscal nf, decimal valor, CancellationToken ct)
    {
        var cliente   = nf.NomeCliente ?? "Cliente";
        var descricao = $"NF {nf.Numero}/{nf.SerieNotaFiscal} — {cliente}";
        var docOrigem = nf.Numero.ToString();
        var vencimento = nf.DataEmissao ?? DateOnly.FromDateTime(DateTime.Today);

        var id = await financeiro.CriarContaReceberAsync(new(
            CdFilial:        nf.CodigoFilial,
            Descricao:       descricao,
            Valor:           valor,
            Vencimento:      vencimento,
            DocumentoOrigem: docOrigem
        ), ct);

        if (id.HasValue)
            logger.LogInformation(
                "NF {Numero}/{Serie}: ContaReceber #{Id} criada no Financeiro (R$ {Valor:N2}).",
                nf.Numero, nf.SerieNotaFiscal, id.Value, valor);
        else
            logger.LogWarning(
                "NF {Numero}/{Serie}: falha ao criar ContaReceber no Financeiro.",
                nf.Numero, nf.SerieNotaFiscal);
    }
}
