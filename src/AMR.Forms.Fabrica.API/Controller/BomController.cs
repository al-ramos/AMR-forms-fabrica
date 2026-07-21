using MediatR;
using Microsoft.AspNetCore.Mvc;
using AMR.Forms.Fabrica.Application.Features.Bom.Commands;
using AMR.Forms.Fabrica.Application.Features.Bom.Queries;

namespace AMR.Forms.Fabrica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
public class BomController(IMediator mediator) : ControllerBase
{
    /// <summary>Retorna a árvore BOM multinível de um produto.</summary>
    [HttpGet("{codigoProduto:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetArvore(int codigoProduto, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetBomQuery(codigoProduto), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Retorna a explosão de materiais (lista achatada com quantidades acumuladas).</summary>
    [HttpGet("{codigoProduto:int}/explosao")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetExplosao(
        int codigoProduto,
        [FromQuery] decimal quantidade = 1,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetExplosaoMaterialQuery(codigoProduto, quantidade), ct);
        return Ok(result);
    }

    /// <summary>Retorna o custo BOM calculado bottom-up.</summary>
    [HttpGet("{codigoProduto:int}/custo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCusto(int codigoProduto, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetCustoBomQuery(codigoProduto), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Adiciona um componente à estrutura BOM de um produto.</summary>
    [HttpPost("{codigoProdutoPai:int}/itens")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AdicionarItem(
        int codigoProdutoPai,
        [FromBody] AdicionarBomItemRequest request,
        CancellationToken ct = default)
    {
        var cmd = new AdicionarBomItemCommand(
            codigoProdutoPai,
            request.CodigoProdutoFilho,
            request.Quantidade,
            request.PercentualPerda);

        var result = await mediator.Send(cmd, ct);
        if (!result.Sucesso)
            return BadRequest(new ProblemDetails { Title = result.Erro });

        return CreatedAtAction(nameof(GetArvore), new { codigoProduto = codigoProdutoPai }, result.Valor);
    }

    /// <summary>Atualiza quantidade e percentual de perda de um item BOM.</summary>
    [HttpPut("{codigoProdutoPai:int}/itens/{codigoProdutoFilho:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AtualizarItem(
        int codigoProdutoPai,
        int codigoProdutoFilho,
        [FromBody] AtualizarBomItemRequest request,
        CancellationToken ct = default)
    {
        var cmd = new AtualizarBomItemCommand(codigoProdutoPai, codigoProdutoFilho, request.Quantidade, request.PercentualPerda);
        var result = await mediator.Send(cmd, ct);

        if (!result.Sucesso)
            return NotFound(new ProblemDetails { Title = result.Erro });

        return NoContent();
    }

    /// <summary>Remove (desativa) um componente da estrutura BOM.</summary>
    [HttpDelete("{codigoProdutoPai:int}/itens/{codigoProdutoFilho:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoverItem(
        int codigoProdutoPai,
        int codigoProdutoFilho,
        CancellationToken ct = default)
    {
        var cmd = new RemoverBomItemCommand(codigoProdutoPai, codigoProdutoFilho);
        var result = await mediator.Send(cmd, ct);

        if (!result.Sucesso)
            return NotFound(new ProblemDetails { Title = result.Erro });

        return NoContent();
    }

    /// <summary>Atualiza dados BOM de um produto (TipoProduto, LeadTime, CustoPadrão).</summary>
    [HttpPut("produtos/{codigoProduto:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AtualizarProduto(
        int codigoProduto,
        [FromBody] AtualizarDadosBomProdutoRequest request,
        CancellationToken ct = default)
    {
        var cmd = new AtualizarDadosBomProdutoCommand(codigoProduto, request.TipoProduto, request.LeadTimeDias, request.CustoPadrao);
        var result = await mediator.Send(cmd, ct);

        if (!result.Sucesso)
            return NotFound(new ProblemDetails { Title = result.Erro });

        return NoContent();
    }
}

public record AdicionarBomItemRequest(int CodigoProdutoFilho, decimal Quantidade, decimal PercentualPerda = 0);
public record AtualizarBomItemRequest(decimal Quantidade, decimal PercentualPerda);
public record AtualizarDadosBomProdutoRequest(string? TipoProduto, int LeadTimeDias, decimal CustoPadrao);
