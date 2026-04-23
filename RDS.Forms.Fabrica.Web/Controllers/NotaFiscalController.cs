using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RDS.Forms.Fabrica.Infrastructure.Data;

namespace RDS.Forms.Fabrica.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotaFiscalController : ControllerBase
{
    private readonly RdsFormasFabricaDbContext _context;

    public NotaFiscalController(RdsFormasFabricaDbContext context)
        => _context = context;

    // GET api/notafiscal
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var nfs = await _context.NotaFiscals
            .Include(n => n.CdFilialNavigation)
            .Include(n => n.CdFichaNavigation)
            .OrderByDescending(n => n.DtEmissaoNf)
            .Take(100)
            .Select(n => new {
                n.CdNotaFiscal,
                n.CdSerNotaFiscal,
                n.CdFilial,
                NoFilial = n.NoFilial ?? n.CdFilialNavigation.NoFilial,
                n.NoCliente,
                n.CdChaveNfe,
                n.CdProtocolo,
                n.DtEmissaoNf,
                n.IcCancelado,
                n.IcImpressoes,
                n.VlTransmissao,
                n.CdAmbiente,
                n.CdModeloNf,
                n.CdFicha,
                n.CdCnpjCliente,
            })
            .ToListAsync();

        return Ok(nfs);
    }

    // GET api/notafiscal/ficha/5
    [HttpGet("ficha/{cdFicha}")]
    public async Task<IActionResult> GetByFicha(int cdFicha)
    {
        var nfs = await _context.NotaFiscals
            .Where(n => n.CdFicha == cdFicha)
            .Select(n => new {
                n.CdNotaFiscal,
                n.CdSerNotaFiscal,
                n.NoCliente,
                n.CdChaveNfe,
                n.DtEmissaoNf,
                n.IcCancelado,
                n.VlTransmissao,
            })
            .ToListAsync();

        return Ok(nfs);
    }

    // GET api/notafiscal/filial/1
    [HttpGet("filial/{cdFilial}")]
    public async Task<IActionResult> GetByFilial(int cdFilial)
    {
        var nfs = await _context.NotaFiscals
            .Where(n => n.CdFilial == cdFilial)
            .OrderByDescending(n => n.DtEmissaoNf)
            .Select(n => new {
                n.CdNotaFiscal,
                n.CdSerNotaFiscal,
                n.NoFilial,
                n.NoCliente,
                n.CdChaveNfe,
                n.DtEmissaoNf,
                n.IcCancelado,
                n.VlTransmissao,
            })
            .ToListAsync();

        return Ok(nfs);
    }

    // GET api/notafiscal/5/001/itens
    [HttpGet("{cdNotaFiscal}/{cdSerNotaFiscal}/itens")]
    public async Task<IActionResult> GetItens(int cdNotaFiscal, string cdSerNotaFiscal)
    {
        var itens = await _context.NotaFiscalDetalhes
            .Include(d => d.CdProdutoNavigation)
            .Where(d => d.CdNotaFiscal == cdNotaFiscal && d.CdSerNotaFiscal == cdSerNotaFiscal)
            .Select(d => new {
                d.CdNotaFiscal,
                d.CdSerNotaFiscal,
                d.CdProduto,
                NoProduto = d.CdProdutoNavigation != null ? d.CdProdutoNavigation.NoProduto : null,
                d.QtProduto,
                d.CdUnidadeMedida,
                d.VlPrecoUnitario,
                d.VlTotal,
                d.VlAliquotaIcms,
                d.VlAliquotaIpi,
                d.VlIpi,
                d.VlPis,
                d.VlConfis,
                d.VlIcmsSt,
                d.CdIcmsCst,
                d.CdEan,
            })
            .ToListAsync();

        return Ok(itens);
    }
}