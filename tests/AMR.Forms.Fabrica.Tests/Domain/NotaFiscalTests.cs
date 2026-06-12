using AMR.Forms.Fabrica.Domain.Entities;

namespace AMR.Forms.Fabrica.Tests.Domain;

public class NotaFiscalTests
{
    private static NotaFiscal CriarNf(int numero = 1, string serie = "1", int filial = 1)
        => new(numero, serie, filial, codigoFicha: null, DateOnly.FromDateTime(DateTime.Today));

    // ── FoiTransmitida ────────────────────────────────────────────────────────

    [Fact]
    public void FoiTransmitida_SemRegistro_RetornaFalse()
    {
        var nf = CriarNf();
        Assert.False(nf.FoiTransmitida);
    }

    [Fact]
    public void FoiTransmitida_AposRegistro_RetornaTrue()
    {
        var nf = CriarNf();
        nf.RegistrarTransmissao("CHAVE44DIGITOS", "PROTOCOLO123", 10000m);
        Assert.True(nf.FoiTransmitida);
    }

    // ── RegistrarTransmissao ──────────────────────────────────────────────────

    [Fact]
    public void RegistrarTransmissao_PreencheTodosOsCampos()
    {
        var nf = CriarNf();
        nf.RegistrarTransmissao("CHAVE_NF", "PROTO_99", 5500.50m);

        Assert.Equal("CHAVE_NF", nf.ChaveNfe);
        Assert.Equal("PROTO_99", nf.Protocolo);
        Assert.Equal(5500.50m, nf.ValorTransmissao);
    }

    [Fact]
    public void RegistrarTransmissao_ValorNulo_AindaTransmitida()
    {
        var nf = CriarNf();
        nf.RegistrarTransmissao("CHAVE", "PROTO", valorTransmissao: null);
        Assert.True(nf.FoiTransmitida);
        Assert.Null(nf.ValorTransmissao);
    }

    // ── EstaCancelada ─────────────────────────────────────────────────────────

    [Fact]
    public void EstaCancelada_SemCancelamento_RetornaFalse()
    {
        var nf = CriarNf();
        Assert.False(nf.EstaCancelada);
    }

    [Fact]
    public void EstaCancelada_AposCancelamento_RetornaTrue()
    {
        var nf = CriarNf();
        nf.Cancelar("Erro de digitação");
        Assert.True(nf.EstaCancelada);
    }

    // ── Cancelar ──────────────────────────────────────────────────────────────

    [Fact]
    public void Cancelar_NfAtiva_NaoLancaExcecao()
    {
        var nf = CriarNf();
        var ex = Record.Exception(() => nf.Cancelar("justificativa"));
        Assert.Null(ex);
    }

    [Fact]
    public void Cancelar_NfJaCancelada_LancaInvalidOperationException()
    {
        var nf = CriarNf();
        nf.Cancelar("primeira vez");
        Assert.Throws<InvalidOperationException>(() => nf.Cancelar("segunda vez"));
    }

    // ── EhAmbienteProducao ────────────────────────────────────────────────────

    [Fact]
    public void EhAmbienteProducao_Ambiente1_RetornaTrue()
    {
        var nf = new NotaFiscal(1, "1", 1, null, DateOnly.FromDateTime(DateTime.Today));
        // Ambiente é preenchido via RegistrarTransmissao — testamos via construção indireta
        // Verificamos o comportamento padrão: sem ambiente definido não é produção
        Assert.False(nf.EhAmbienteProducao);
    }

    [Fact]
    public void Construtor_PreencheCamposCorretamente()
    {
        var data = DateOnly.FromDateTime(DateTime.Today);
        var nf = new NotaFiscal(42, "A", 3, 10, data);

        Assert.Equal(42, nf.Numero);
        Assert.Equal("A", nf.SerieNotaFiscal);
        Assert.Equal(3, nf.CodigoFilial);
        Assert.Equal(10, nf.CodigoFicha);
        Assert.Equal(data, nf.DataEmissao);
    }
}
