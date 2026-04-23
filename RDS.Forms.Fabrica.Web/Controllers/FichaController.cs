using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RDS.Forms.Fabrica.Infrastructure.Data;
using RDS.Forms.Fabrica.Infrastructure.Data.Entities;

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

    // POST api/ficha
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Ficha novaFicha)
    {
        if (novaFicha == null)
            return BadRequest("Os dados da ficha não podem ser nulos.");

        try
        {
            // Adiciona a nova ficha ao contexto do Entity Framework
            _context.Fichas.Add(novaFicha);

            // Salva as alterações no banco de dados
            await _context.SaveChangesAsync();

            // Retorna o status 201 (Created) e a rota para buscar a ficha criada
            return CreatedAtAction(nameof(GetById), new { id = novaFicha.CdFicha }, novaFicha);
        }
        catch (Exception ex)
        {
            // Retorna 500 caso dê algum erro de banco (ex: chave estrangeira inválida)
            //return StatusCode(500, $"Erro interno ao criar ficha: {ex.Message}");            
            // Pega a mensagem real do banco de dados (InnerException) se ela existir
            var mensagemErro = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return StatusCode(500, $"Erro interno ao criar ficha: {mensagemErro}");
        }
    
    }

}