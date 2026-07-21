using AMR.Forms.Fabrica.Application.Common;
using AMR.Forms.Fabrica.Application.Features.Manutencao.Commands;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;
using MediatR;

namespace AMR.Forms.Fabrica.Application.Features.Manutencao.Handlers;

public class CriarPlanoManutencaoHandler(IPlanoManutencaoRepository repo, IEquipamentoRepository equipRepo, IUnitOfWork uow)
    : IRequestHandler<CriarPlanoManutencaoCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CriarPlanoManutencaoCommand request, CancellationToken ct)
    {
        var eq = await equipRepo.ObterPorIdAsync(request.EquipamentoId);
        if (eq is null) return Result<int>.Falha($"Equipamento {request.EquipamentoId} não encontrado.");
        if (!eq.Ativo)  return Result<int>.Falha($"Equipamento '{eq.Nome}' está inativo.");

        try
        {
            var plano = new PlanoManutencao(
                request.EquipamentoId, request.CodigoFilial, request.TipoManutencao,
                request.Descricao, request.Instrucoes, request.FrequenciaDias,
                request.DuracaoEstimadaHoras, request.ProximaExecucao);

            await repo.AdicionarAsync(plano);
            await uow.SaveChangesAsync(ct);
            return Result<int>.Ok(plano.Id);
        }
        catch (ArgumentException ex) { return Result<int>.Falha(ex.Message); }
    }
}

public class AtualizarPlanoManutencaoHandler(IPlanoManutencaoRepository repo, IUnitOfWork uow)
    : IRequestHandler<AtualizarPlanoManutencaoCommand, Result>
{
    public async Task<Result> Handle(AtualizarPlanoManutencaoCommand request, CancellationToken ct)
    {
        var plano = await repo.ObterPorIdAsync(request.Id);
        if (plano is null) return Result.Falha($"Plano {request.Id} não encontrado.");
        try
        {
            plano.Atualizar(request.Descricao, request.Instrucoes, request.FrequenciaDias, request.DuracaoEstimadaHoras);
            await repo.AtualizarAsync(plano);
            await uow.SaveChangesAsync(ct);
            return Result.Ok();
        }
        catch (ArgumentException ex) { return Result.Falha(ex.Message); }
    }
}

public class AlterarStatusPlanoHandler(IPlanoManutencaoRepository repo, IUnitOfWork uow)
    : IRequestHandler<AlterarStatusPlanoCommand, Result>
{
    public async Task<Result> Handle(AlterarStatusPlanoCommand request, CancellationToken ct)
    {
        var plano = await repo.ObterPorIdAsync(request.Id);
        if (plano is null) return Result.Falha($"Plano {request.Id} não encontrado.");
        if (request.Ativo) plano.Ativar(); else plano.Desativar();
        await repo.AtualizarAsync(plano);
        await uow.SaveChangesAsync(ct);
        return Result.Ok();
    }
}

public class AbrirOrdemManutencaoHandler(IOrdemManutencaoRepository repo, IEquipamentoRepository equipRepo, IUnitOfWork uow)
    : IRequestHandler<AbrirOrdemManutencaoCommand, Result<int>>
{
    public async Task<Result<int>> Handle(AbrirOrdemManutencaoCommand request, CancellationToken ct)
    {
        var eq = await equipRepo.ObterPorIdAsync(request.EquipamentoId);
        if (eq is null) return Result<int>.Falha($"Equipamento {request.EquipamentoId} não encontrado.");
        try
        {
            var om = new OrdemManutencao(
                request.EquipamentoId, request.CodigoFilial, request.TipoManutencao,
                request.Descricao, request.DataPrevista, request.CodigoTecnico,
                request.Observacao, request.PlanoManutencaoId);

            await repo.AdicionarAsync(om);
            await uow.SaveChangesAsync(ct);
            return Result<int>.Ok(om.Id);
        }
        catch (ArgumentException ex) { return Result<int>.Falha(ex.Message); }
    }
}

public class IniciarOrdemManutencaoHandler(IOrdemManutencaoRepository repo, IUnitOfWork uow)
    : IRequestHandler<IniciarOrdemManutencaoCommand, Result>
{
    public async Task<Result> Handle(IniciarOrdemManutencaoCommand request, CancellationToken ct)
    {
        var om = await repo.ObterPorIdAsync(request.Id);
        if (om is null) return Result.Falha($"OM {request.Id} não encontrada.");
        try { om.IniciarExecucao(request.CodigoTecnico); }
        catch (InvalidOperationException ex) { return Result.Falha(ex.Message); }
        await repo.AtualizarAsync(om);
        await uow.SaveChangesAsync(ct);
        return Result.Ok();
    }
}

public class ConcluirOrdemManutencaoHandler(IOrdemManutencaoRepository repo, IPlanoManutencaoRepository planoRepo, IUnitOfWork uow)
    : IRequestHandler<ConcluirOrdemManutencaoCommand, Result>
{
    public async Task<Result> Handle(ConcluirOrdemManutencaoCommand request, CancellationToken ct)
    {
        var om = await repo.ObterPorIdAsync(request.Id);
        if (om is null) return Result.Falha($"OM {request.Id} não encontrada.");
        try { om.Concluir(request.DuracaoRealHoras, request.Observacao); }
        catch (InvalidOperationException ex) { return Result.Falha(ex.Message); }
        catch (ArgumentException ex)         { return Result.Falha(ex.Message); }

        await repo.AtualizarAsync(om);

        // Avança ProximaExecucao do plano, se vinculado
        if (om.PlanoManutencaoId.HasValue)
        {
            var plano = await planoRepo.ObterPorIdAsync(om.PlanoManutencaoId.Value);
            if (plano is not null)
            {
                plano.RegistrarExecucao(om.DataConclusao!.Value);
                await planoRepo.AtualizarAsync(plano);
            }
        }

        await uow.SaveChangesAsync(ct);
        return Result.Ok();
    }
}

public class CancelarOrdemManutencaoHandler(IOrdemManutencaoRepository repo, IUnitOfWork uow)
    : IRequestHandler<CancelarOrdemManutencaoCommand, Result>
{
    public async Task<Result> Handle(CancelarOrdemManutencaoCommand request, CancellationToken ct)
    {
        var om = await repo.ObterPorIdAsync(request.Id);
        if (om is null) return Result.Falha($"OM {request.Id} não encontrada.");
        try { om.Cancelar(request.Motivo); }
        catch (InvalidOperationException ex) { return Result.Falha(ex.Message); }
        await repo.AtualizarAsync(om);
        await uow.SaveChangesAsync(ct);
        return Result.Ok();
    }
}
