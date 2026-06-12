using AMR.Forms.Fabrica.Domain.Entities;

namespace AMR.Forms.Fabrica.Tests.Domain;

public class FichaTests
{
    private static Ficha CriarFicha(int codigo = 1, int filial = 1)
        => new(codigo, filial, "ABC-1234", "BU01", 10, "João Silva", DateOnly.FromDateTime(DateTime.Today));

    // ── Construtor ────────────────────────────────────────────────────────────

    [Fact]
    public void Construtor_CodigoNegativo_LancaArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() => CriarFicha(codigo: -1));
        Assert.Contains("Código", ex.Message);
    }

    [Fact]
    public void Construtor_CodigoZero_CriaComSucesso()
    {
        var ficha = CriarFicha(codigo: 0);
        Assert.Equal(0, ficha.Codigo);
    }

    [Fact]
    public void Construtor_PreencheCamposCorretamente()
    {
        var ficha = new Ficha(42, 2, "XYZ-9999", "BU02", 5, "Maria", DateOnly.FromDateTime(DateTime.Today));

        Assert.Equal(42, ficha.Codigo);
        Assert.Equal(2, ficha.CodigoFilial);
        Assert.Equal("XYZ-9999", ficha.PlacaVeiculo);
        Assert.Equal("BU02", ficha.CodigoBusinessUnit);
    }

    // ── EstaFinalizada / IntegradaComJde ──────────────────────────────────────

    [Fact]
    public void EstaFinalizada_SemDataSaida_RetornaFalse()
    {
        var ficha = CriarFicha();
        Assert.False(ficha.EstaFinalizada);
    }

    [Fact]
    public void IntegradaComJde_SemData_RetornaFalse()
    {
        var ficha = CriarFicha();
        Assert.False(ficha.IntegradaComJde);
    }

    // ── AvancarPasso ─────────────────────────────────────────────────────────

    [Fact]
    public void AvancarPasso_AtualizaCodigoPassoAtual()
    {
        var ficha = CriarFicha();
        ficha.AvancarPasso(3);
        Assert.Equal(3, ficha.CodigoPassoAtual);
    }

    [Fact]
    public void AvancarPasso_MultiplosPassos_MantemUltimo()
    {
        var ficha = CriarFicha();
        ficha.AvancarPasso(1);
        ficha.AvancarPasso(2);
        ficha.AvancarPasso(5);
        Assert.Equal(5, ficha.CodigoPassoAtual);
    }

    // ── Finalizar ─────────────────────────────────────────────────────────────

    [Fact]
    public void Finalizar_FichaAberta_SetaDataSaidaEEstaFinalizada()
    {
        var ficha = CriarFicha();
        ficha.Finalizar();
        Assert.True(ficha.EstaFinalizada);
        Assert.Equal(DateOnly.FromDateTime(DateTime.Today), ficha.DataSaida);
    }

    [Fact]
    public void Finalizar_FichaJaFinalizada_LancaInvalidOperationException()
    {
        var ficha = CriarFicha();
        ficha.Finalizar();
        Assert.Throws<InvalidOperationException>(() => ficha.Finalizar());
    }

    // ── MarcarIntegracaoJde ───────────────────────────────────────────────────

    [Fact]
    public void MarcarIntegracaoJde_SetaDataEIntegrada()
    {
        var ficha = CriarFicha();
        ficha.MarcarIntegracaoJde();
        Assert.True(ficha.IntegradaComJde);
        Assert.Equal(DateOnly.FromDateTime(DateTime.Today), ficha.DataInterfaceJde);
    }
}
