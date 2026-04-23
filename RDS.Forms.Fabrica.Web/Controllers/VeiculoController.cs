using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RDS.Forms.Fabrica.Infrastructure.Data;
using RDS.Forms.Fabrica.Infrastructure.Data.Entities;

namespace RDS.Forms.Fabrica.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VeiculoController : ControllerBase
{
    private readonly RdsFormasFabricaDbContext _context;

    public VeiculoController(RdsFormasFabricaDbContext context)
        => _context = context;

    // GET api/veiculo/filial/1
    [HttpGet("filial/{cdFilial}")]
    public async Task<IActionResult> GetByFilial(int cdFilial)
    {
        var veiculos = await _context.Veiculos
            .Where(v => v.CdFilial == cdFilial)
            .Select(v => new { v.CdPlacaVeiculo, v.CdFilial })
            .ToListAsync();

        return Ok(veiculos);
    }

    // GET api/veiculo
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var veiculos = await _context.Veiculos
            .Select(v => new { v.CdPlacaVeiculo, v.CdFilial })
            .ToListAsync();

        return Ok(veiculos);
    }

    // POST api/veiculo
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] NovoVeiculoDto dto)
    {
        var veiculo = new Veiculo
        {
            CdPlacaVeiculo = dto.CdPlacaVeiculo.ToUpper().Trim(),
            CdFilial = dto.CdFilial
        };

        _context.Veiculos.Add(veiculo);
        await _context.SaveChangesAsync();

        return Ok(veiculo);
    }

    public record NovoVeiculoDto(string CdPlacaVeiculo, int CdFilial);
}