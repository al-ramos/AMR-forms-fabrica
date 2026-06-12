using AMR.Forms.Fabrica.Domain.Entities;

namespace AMR.Forms.Fabrica.Tests.Domain;

public class VeiculoTests
{
    // ── Construtor ────────────────────────────────────────────────────────────

    [Fact]
    public void Construtor_PlacaVazia_LancaArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Veiculo("", 1, "SP", null));
    }

    [Fact]
    public void Construtor_PlacaNula_LancaArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Veiculo(null!, 1, "SP", null));
    }

    [Fact]
    public void Construtor_PlacaEspacos_LancaArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Veiculo("   ", 1, "SP", null));
    }

    [Fact]
    public void Construtor_PlacaMinuscula_NormalizaParaUppercase()
    {
        var veiculo = new Veiculo("abc-1234", 1, "SP", null);
        Assert.Equal("ABC-1234", veiculo.Placa);
    }

    [Fact]
    public void Construtor_PlacaValida_PreencheCampos()
    {
        var veiculo = new Veiculo("ABC-1234", 2, "MG", "RNTC-001");

        Assert.Equal("ABC-1234", veiculo.Placa);
        Assert.Equal(2, veiculo.CodigoFilial);
        Assert.Equal("MG", veiculo.UfVeiculo);
        Assert.Equal("RNTC-001", veiculo.CodigoRntc);
    }

    // ── Atualizar ─────────────────────────────────────────────────────────────

    [Fact]
    public void Atualizar_ModificaTodosOsCampos()
    {
        var veiculo = new Veiculo("ABC-1234", 1, "SP", "OLD-RNTC");
        veiculo.Atualizar(3, "RJ", "NEW-RNTC");

        Assert.Equal(3, veiculo.CodigoFilial);
        Assert.Equal("RJ", veiculo.UfVeiculo);
        Assert.Equal("NEW-RNTC", veiculo.CodigoRntc);
    }

    [Fact]
    public void Atualizar_ComNulos_MantemPlaca()
    {
        var veiculo = new Veiculo("XYZ-9999", 1, "SP", "RNTC");
        veiculo.Atualizar(2, null, null);

        Assert.Equal("XYZ-9999", veiculo.Placa); // placa não muda
        Assert.Null(veiculo.UfVeiculo);
        Assert.Null(veiculo.CodigoRntc);
    }
}
