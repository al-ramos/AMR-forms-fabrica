using AMR.Forms.Fabrica.Domain.Entities;

namespace AMR.Forms.Fabrica.Tests.Domain;

public class FichaBalancaTests
{
    // ── PesoLiquido ───────────────────────────────────────────────────────────

    [Fact]
    public void PesoLiquido_ComAmbosOsPesos_RetornaValorAbsoluto()
    {
        var balanca = new FichaBalanca(1, 1, null, "O", peso1: 5000m, peso2: 2000m);
        Assert.Equal(3000m, balanca.PesoLiquido);
    }

    [Fact]
    public void PesoLiquido_ComPesoMenorNoPeso1_RetornaSemprePositivo()
    {
        var balanca = new FichaBalanca(1, 1, null, "D", peso1: 1500m, peso2: 4000m);
        Assert.Equal(2500m, balanca.PesoLiquido);
    }

    [Fact]
    public void PesoLiquido_SemPeso1_RetornaNull()
    {
        var balanca = new FichaBalanca(1, 1, null, "O", peso1: null, peso2: 3000m);
        Assert.Null(balanca.PesoLiquido);
    }

    [Fact]
    public void PesoLiquido_SemPeso2_RetornaNull()
    {
        var balanca = new FichaBalanca(1, 1, null, "O", peso1: 3000m, peso2: null);
        Assert.Null(balanca.PesoLiquido);
    }

    [Fact]
    public void PesoLiquido_SemNenhumPeso_RetornaNull()
    {
        var balanca = new FichaBalanca(1, 1, null, null, peso1: null, peso2: null);
        Assert.Null(balanca.PesoLiquido);
    }

    [Fact]
    public void PesoLiquido_PesosIguais_RetornaZero()
    {
        var balanca = new FichaBalanca(1, 1, null, "O", peso1: 2500m, peso2: 2500m);
        Assert.Equal(0m, balanca.PesoLiquido);
    }

    // ── Construtor / propriedades ─────────────────────────────────────────────

    [Fact]
    public void Construtor_PreencheCamposCorretamente()
    {
        var balanca = new FichaBalanca(10, 2, 99, "D", 6000m, 1500m);

        Assert.Equal(10, balanca.CodigoFicha);
        Assert.Equal(2, balanca.CodigoFilial);
        Assert.Equal(99, balanca.CodigoPesagem);
        Assert.Equal("D", balanca.OrigemDestino);
    }
}
