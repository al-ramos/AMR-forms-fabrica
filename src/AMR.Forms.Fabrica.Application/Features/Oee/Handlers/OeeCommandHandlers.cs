using AMR.Forms.Fabrica.Application.Common;
using AMR.Forms.Fabrica.Application.Features.Oee.Commands;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;
using MediatR;

namespace AMR.Forms.Fabrica.Application.Features.Oee.Handlers;

public class CadastrarEquipamentoHandler(IEquipamentoRepository repo, IUnitOfWork uow)
    : IRequestHandler<CadastrarEquipamentoCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CadastrarEquipamentoCommand request, CancellationToken ct)
    {
        try
        {
            var eq = new Equipamento(request.CodigoFilial, request.Nome, request.Descricao, request.CodigoArea);
            await repo.AdicionarAsync(eq);
            await uow.SaveChangesAsync(ct);
            return Result<int>.Ok(eq.Id);
        }
        catch (ArgumentException ex) { return Result<int>.Falha(ex.Message); }
    }
}

public class AtualizarEquipamentoHandler(IEquipamentoRepository repo, IUnitOfWork uow)
    : IRequestHandler<AtualizarEquipamentoCommand, Result>
{
    public async Task<Result> Handle(AtualizarEquipamentoCommand request, CancellationToken ct)
    {
        var eq = await repo.ObterPorIdAsync(request.Id);
        if (eq is null) return Result.Falha($"Equipamento {request.Id} não encontrado.");
        try
        {
            eq.Atualizar(request.Nome, request.Descricao, request.CodigoArea);
            await repo.AtualizarAsync(eq);
            await uow.SaveChangesAsync(ct);
            return Result.Ok();
        }
        catch (ArgumentException ex) { return Result.Falha(ex.Message); }
    }
}

public class AlterarStatusEquipamentoHandler(IEquipamentoRepository repo, IUnitOfWork uow)
    : IRequestHandler<AlterarStatusEquipamentoCommand, Result>
{
    public async Task<Result> Handle(AlterarStatusEquipamentoCommand request, CancellationToken ct)
    {
        var eq = await repo.ObterPorIdAsync(request.Id);
        if (eq is null) return Result.Falha($"Equipamento {request.Id} não encontrado.");

        if (request.Ativo) eq.Ativar(); else eq.Desativar();
        await repo.AtualizarAsync(eq);
        await uow.SaveChangesAsync(ct);
        return Result.Ok();
    }
}

public class RegistrarOeeHandler(IRegistroOeeRepository oeeRepo, IEquipamentoRepository equipRepo, IUnitOfWork uow)
    : IRequestHandler<RegistrarOeeCommand, Result<int>>
{
    public async Task<Result<int>> Handle(RegistrarOeeCommand request, CancellationToken ct)
    {
        var eq = await equipRepo.ObterPorIdAsync(request.EquipamentoId);
        if (eq is null)
            return Result<int>.Falha($"Equipamento {request.EquipamentoId} não encontrado.");
        if (!eq.Ativo)
            return Result<int>.Falha($"Equipamento '{eq.Nome}' está inativo.");

        try
        {
            var registro = new RegistroOee(
                request.EquipamentoId,
                request.CodigoFilial,
                request.DataHoraInicio,
                request.DataHoraFim,
                request.TempoPlanejadoMinutos,
                request.TempoRealProducaoMinutos,
                request.QuantidadeProduzida,
                request.QuantidadeAprovada,
                request.TempoCicloIdealSegundos,
                request.CodigoOperador,
                request.Observacao);

            await oeeRepo.AdicionarAsync(registro);
            await uow.SaveChangesAsync(ct);
            return Result<int>.Ok(registro.Id);
        }
        catch (ArgumentException ex) { return Result<int>.Falha(ex.Message); }
    }
}
