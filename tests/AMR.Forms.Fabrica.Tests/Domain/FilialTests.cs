using AMR.Forms.Fabrica.Domain.Entities;

namespace AMR.Forms.Fabrica.Tests.Domain;

public class FilialTests
{
    // ── Construtor ────────────────────────────────────────────────────────────

    [Fact]
    public void Construtor_PreencheCamposCorretamente()
    {
        var filial = new Filial(1, "Matriz SP", "BU-DEP-01", 1);

        Assert.Equal(1, filial.Codigo);
        Assert.Equal("Matriz SP", filial.Nome);
        Assert.Equal("BU-DEP-01", filial.CodigoBuDeposito);
        Assert.Equal(1, filial.TipoImpressaoNf);
    }

    [Fact]
    public void Construtor_NomeNulo_CriaComSucesso()
    {
        var filial = new Filial(2, null, null, null);

        Assert.Equal(2, filial.Codigo);
        Assert.Null(filial.Nome);
        Assert.Null(filial.CodigoBuDeposito);
        Assert.Null(filial.TipoImpressaoNf);
    }

    [Fact]
    public void Construtor_CodigoZero_CriaComSucesso()
    {
        var filial = new Filial(0, "Filial Zero", null, null);
        Assert.Equal(0, filial.Codigo);
    }

    [Fact]
    public void Construtor_TipoImpressaoNfDefinido_Preserva()
    {
        var filial = new Filial(3, "Filial RJ", "BU-DEP-02", 2);
        Assert.Equal(2, filial.TipoImpressaoNf);
    }

    [Fact]
    public void Construtor_MultiplasFiliaisComCodigosDiferentes_SaoDiferentes()
    {
        var f1 = new Filial(1, "SP", null, null);
        var f2 = new Filial(2, "RJ", null, null);

        Assert.NotEqual(f1.Codigo, f2.Codigo);
        Assert.NotEqual(f1.Nome, f2.Nome);
    }
}
