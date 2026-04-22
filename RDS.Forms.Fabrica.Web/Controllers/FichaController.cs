using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RDS.Forms.Fabrica.Infrastructure.Data;

namespace RDS.Forms.Fabrica.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FichaController : ControllerBase
{
    private readonly RdsFormasFabricaDbContext _context;

    public FichaController(RdsFormasFabricaDbContext context)
        => _context = context;

    // GET api/ficha
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var fichas = await _context.Fichas
            .Include(f => f.CdFilialNavigation)
            .Include(f => f.CdTipoOperacaoNavigation)
            .Include(f => f.CdPassoAtualNavigation)
            .OrderByDescending(f => f.DtFicha)
            .Take(50)
            .ToListAsync();

        return Ok(fichas);
    }

    // GET api/ficha/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ficha = await _context.Fichas
            .Include(f => f.CdFilialNavigation)
            .Include(f => f.CdTipoOperacaoNavigation)
            .Include(f => f.CdPassoAtualNavigation)
            .Include(f => f.NotaFiscals)
            .FirstOrDefaultAsync(f => f.CdFicha == id);

        if (ficha is null)
            return NotFound();

        return Ok(ficha);
    }

    // GET api/ficha/filial/1
    [HttpGet("filial/{cdFilial}")]
    public async Task<IActionResult> GetByFilial(int cdFilial)
    {
        var fichas = await _context.Fichas
            .Where(f => f.CdFilial == cdFilial)
            .OrderByDescending(f => f.DtFicha)
            .ToListAsync();

        return Ok(fichas);
    }
}