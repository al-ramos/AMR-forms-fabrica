using AMR.Forms.Fabrica.Domain.Entities;

namespace AMR.Forms.Fabrica.Tests.Domain;

public class LogSistemaTests
{
    // ── CriarErro ─────────────────────────────────────────────────────────────

    [Fact]
    public void CriarErro_SetaTipoLog1_EPendente1()
    {
        var log = LogSistema.CriarErro(1, "Falha na transmissão NF", "operador");
        Assert.Equal(1, log.TipoLog);
        Assert.Equal(1, log.Pendente);
    }

    [Fact]
    public void CriarErro_SetaDescricaoEUsuario()
    {
        var log = LogSistema.CriarErro(2, "Timeout ERP", "admin", codigoFicha: 99);
        Assert.Equal("Timeout ERP", log.DescricaoErro);
        Assert.Equal("admin", log.Usuario);
        Assert.Equal(99, log.CodigoFicha);
    }

    [Fact]
    public void CriarErro_SetaCodigoFilial()
    {
        var log = LogSistema.CriarErro(5, "erro", null);
        Assert.Equal(5, log.CodigoFilial);
    }

    [Fact]
    public void CriarErro_SemCodigoFicha_CodigoFichaNull()
    {
        var log = LogSistema.CriarErro(1, "erro", "user");
        Assert.Null(log.CodigoFicha);
    }

    [Fact]
    public void CriarErro_SetaDataLog()
    {
        var antes = DateTime.Now.AddSeconds(-1);
        var log = LogSistema.CriarErro(1, "erro", null);
        Assert.True(log.DataLog >= antes);
    }

    // ── CriarInfo ─────────────────────────────────────────────────────────────

    [Fact]
    public void CriarInfo_SetaTipoLog0_EPendente0()
    {
        var log = LogSistema.CriarInfo(1, "Sincronização concluída", "sistema");
        Assert.Equal(0, log.TipoLog);
        Assert.Equal(0, log.Pendente);
    }

    [Fact]
    public void CriarInfo_SetaDescricaoEUsuario()
    {
        var log = LogSistema.CriarInfo(1, "Pedido importado", "batch", codigoFicha: 10);
        Assert.Equal("Pedido importado", log.DescricaoErro);
        Assert.Equal("batch", log.Usuario);
        Assert.Equal(10, log.CodigoFicha);
    }

    // ── EhPendente ────────────────────────────────────────────────────────────

    [Fact]
    public void EhPendente_LogDeErro_RetornaTrue()
    {
        var log = LogSistema.CriarErro(1, "erro", null);
        Assert.True(log.EhPendente);
    }

    [Fact]
    public void EhPendente_LogDeInfo_RetornaFalse()
    {
        var log = LogSistema.CriarInfo(1, "info", null);
        Assert.False(log.EhPendente);
    }
}
