using AMR.Forms.Fabrica.Application.Common;
using AMR.Forms.Fabrica.Application.Features.Bom.Commands;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;
using MediatR;

namespace AMR.Forms.Fabrica.Application.Features.Bom.Handlers;

public class AdicionarBomItemHandler(IBomRepository bomRepo, IProdutoBomRepository produtoRepo, IUnitOfWork uow)
    : IRequestHandler<AdicionarBomItemCommand, Result<int>>
{
    public async Task<Result<int>> Handle(AdicionarBomItemCommand request, CancellationToken ct)
    {
        // Validar existência dos produtos
        var pai  = await produtoRepo.ObterComDadosBomAsync(request.CodigoProdutoPai);
        var filho = await produtoRepo.ObterComDadosBomAsync(request.CodigoProdutoFilho);

        if (pai is null)
            return Result<int>.Falha($"Produto pai {request.CodigoProdutoPai} não encontrado.");
        if (filho is null)
            return Result<int>.Falha($"Produto filho {request.CodigoProdutoFilho} não encontrado.");

        // Verificar referência circular
        var circular = await bomRepo.ExisteReferenciaCircularAsync(request.CodigoProdutoPai, request.CodigoProdutoFilho);
        if (circular)
            return Result<int>.Falha("Referência circular detectada: o produto filho já é ancestral do produto pai na estrutura BOM.");

        // Verificar se já existe
        var existente = await bomRepo.ObterItemAsync(request.CodigoProdutoPai, request.CodigoProdutoFilho);
        if (existente is not null && existente.Ativo)
            return Result<int>.Falha("Este componente já existe na estrutura BOM do produto pai. Use a atualização para modificar quantidade.");

        // Calcular nível: pai nível 1 → filhos nível 2, etc.
        var itensPai = await bomRepo.ListarItensPorProdutoPaiAsync(request.CodigoProdutoPai);
        int nivel = itensPai.Any() ? itensPai.Max(i => i.Nivel) : 1;

        var item = new BomItem(
            request.CodigoProdutoPai,
            request.CodigoProdutoFilho,
            request.Quantidade,
            nivel + 1,
            request.PercentualPerda
        );

        await bomRepo.AdicionarAsync(item);
        await uow.SaveChangesAsync(ct);

        return Result<int>.Ok(item.Id);
    }
}

public class RemoverBomItemHandler(IBomRepository bomRepo, IUnitOfWork uow)
    : IRequestHandler<RemoverBomItemCommand, Result>
{
    public async Task<Result> Handle(RemoverBomItemCommand request, CancellationToken ct)
    {
        var item = await bomRepo.ObterItemAsync(request.CodigoProdutoPai, request.CodigoProdutoFilho);
        if (item is null || !item.Ativo)
            return Result.Falha("Item BOM não encontrado ou já removido.");

        item.Desativar();
        await bomRepo.AtualizarAsync(item);
        await uow.SaveChangesAsync(ct);

        return Result.Ok();
    }
}

public class AtualizarBomItemHandler(IBomRepository bomRepo, IUnitOfWork uow)
    : IRequestHandler<AtualizarBomItemCommand, Result>
{
    public async Task<Result> Handle(AtualizarBomItemCommand request, CancellationToken ct)
    {
        var item = await bomRepo.ObterItemAsync(request.CodigoProdutoPai, request.CodigoProdutoFilho);
        if (item is null || !item.Ativo)
            return Result.Falha("Item BOM não encontrado.");

        item.AtualizarQuantidade(request.Quantidade, request.PercentualPerda);
        await bomRepo.AtualizarAsync(item);
        await uow.SaveChangesAsync(ct);

        return Result.Ok();
    }
}

public class AtualizarDadosBomProdutoHandler(IProdutoBomRepository produtoRepo, IUnitOfWork uow)
    : IRequestHandler<AtualizarDadosBomProdutoCommand, Result>
{
    public async Task<Result> Handle(AtualizarDadosBomProdutoCommand request, CancellationToken ct)
    {
        var produto = await produtoRepo.ObterComDadosBomAsync(request.CodigoProduto);
        if (produto is null)
            return Result.Falha($"Produto {request.CodigoProduto} não encontrado.");

        produto.AtualizarDadosBom(request.TipoProduto, request.LeadTimeDias, request.CustoPadrao);
        await produtoRepo.AtualizarDadosBomAsync(produto);
        await uow.SaveChangesAsync(ct);

        return Result.Ok();
    }
}
