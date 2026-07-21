using AMR.Forms.Fabrica.Application.Common;
using AMR.Forms.Fabrica.Application.Features.OrdensProducao.Commands;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;
using MediatR;

namespace AMR.Forms.Fabrica.Application.Features.OrdensProducao.Handlers;

public class AbrirOrdemProducaoHandler(IOrdemProducaoRepository repo, IUnitOfWork uow)
    : IRequestHandler<AbrirOrdemProducaoCommand, Result<int>>
{
    public async Task<Result<int>> Handle(AbrirOrdemProducaoCommand request, CancellationToken ct)
    {
        if (await repo.NumeroJaExisteAsync(request.Numero))
            return Result<int>.Falha($"Número de OP '{request.Numero}' já existe.");

        var op = new OrdemProducao(
            request.Numero, request.CodigoProduto, request.CodigoFilial,
            request.QuantidadePlanejada, request.DataPrevistaFim, request.Observacao);

        await repo.AdicionarAsync(op);
        await uow.SaveChangesAsync(ct);
        return Result<int>.Ok(op.Id);
    }
}

public class LiberarOpHandler(IOrdemProducaoRepository repo, IUnitOfWork uow)
    : IRequestHandler<LiberarOpCommand, Result>
{
    public async Task<Result> Handle(LiberarOpCommand request, CancellationToken ct)
    {
        var op = await repo.ObterPorIdAsync(request.Id);
        if (op is null) return Result.Falha($"OP {request.Id} não encontrada.");
        try { op.Liberar(); } catch (Exception ex) { return Result.Falha(ex.Message); }
        await repo.AtualizarAsync(op);
        await uow.SaveChangesAsync(ct);
        return Result.Ok();
    }
}

public class IniciarProducaoHandler(IOrdemProducaoRepository repo, IUnitOfWork uow)
    : IRequestHandler<IniciarProducaoCommand, Result>
{
    public async Task<Result> Handle(IniciarProducaoCommand request, CancellationToken ct)
    {
        var op = await repo.ObterPorIdAsync(request.Id);
        if (op is null) return Result.Falha($"OP {request.Id} não encontrada.");
        try { op.IniciarProducao(); } catch (Exception ex) { return Result.Falha(ex.Message); }
        await repo.AtualizarAsync(op);
        await uow.SaveChangesAsync(ct);
        return Result.Ok();
    }
}

public class RegistrarProducaoHandler(IOrdemProducaoRepository opRepo, IRastreabilidadeRepository rastRepo, IUnitOfWork uow)
    : IRequestHandler<RegistrarProducaoCommand, Result>
{
    public async Task<Result> Handle(RegistrarProducaoCommand request, CancellationToken ct)
    {
        var op = await opRepo.ObterPorIdAsync(request.OrdemProducaoId);
        if (op is null) return Result.Falha($"OP {request.OrdemProducaoId} não encontrada.");

        try { op.RegistrarProducao(request.Quantidade); } catch (Exception ex) { return Result.Falha(ex.Message); }

        var rast = new RastreabilidadeItem(
            op.Id, op.CodigoProduto, request.Lote, request.Quantidade,
            TipoMovimentoRastreabilidade.Producao, request.CodigoOperador, request.Observacao);

        await opRepo.AtualizarAsync(op);
        await rastRepo.AdicionarAsync(rast);
        await uow.SaveChangesAsync(ct);
        return Result.Ok();
    }
}

public class RegistrarRejeicaoHandler(IOrdemProducaoRepository opRepo, IRastreabilidadeRepository rastRepo, IUnitOfWork uow)
    : IRequestHandler<RegistrarRejeicaoCommand, Result>
{
    public async Task<Result> Handle(RegistrarRejeicaoCommand request, CancellationToken ct)
    {
        var op = await opRepo.ObterPorIdAsync(request.OrdemProducaoId);
        if (op is null) return Result.Falha($"OP {request.OrdemProducaoId} não encontrada.");

        try { op.RegistrarRejeicao(request.Quantidade); } catch (Exception ex) { return Result.Falha(ex.Message); }

        var rast = new RastreabilidadeItem(
            op.Id, op.CodigoProduto, request.Lote, request.Quantidade,
            TipoMovimentoRastreabilidade.Rejeicao, request.CodigoOperador, request.MotivoRejeicao);

        await opRepo.AtualizarAsync(op);
        await rastRepo.AdicionarAsync(rast);
        await uow.SaveChangesAsync(ct);
        return Result.Ok();
    }
}

public class ConcluirOpHandler(IOrdemProducaoRepository repo, IUnitOfWork uow)
    : IRequestHandler<ConcluirOpCommand, Result>
{
    public async Task<Result> Handle(ConcluirOpCommand request, CancellationToken ct)
    {
        var op = await repo.ObterPorIdAsync(request.Id);
        if (op is null) return Result.Falha($"OP {request.Id} não encontrada.");
        try { op.Concluir(); } catch (Exception ex) { return Result.Falha(ex.Message); }
        await repo.AtualizarAsync(op);
        await uow.SaveChangesAsync(ct);
        return Result.Ok();
    }
}

public class CancelarOpHandler(IOrdemProducaoRepository repo, IUnitOfWork uow)
    : IRequestHandler<CancelarOpCommand, Result>
{
    public async Task<Result> Handle(CancelarOpCommand request, CancellationToken ct)
    {
        var op = await repo.ObterPorIdAsync(request.Id);
        if (op is null) return Result.Falha($"OP {request.Id} não encontrada.");
        try { op.Cancelar(request.Motivo); } catch (Exception ex) { return Result.Falha(ex.Message); }
        await repo.AtualizarAsync(op);
        await uow.SaveChangesAsync(ct);
        return Result.Ok();
    }
}
