namespace RDS.Forms.Fabrica.Domain.Entities;

/// <summary>
/// Registro de log de erros e eventos do sistema por filial e ficha.
/// </summary>
public class LogSistema
{
    public int CodigoFilial { get; private set; }
    public int? TipoLog { get; private set; }
    public int? Pendente { get; private set; }
    public DateTime? DataLog { get; private set; }
    public string? Usuario { get; private set; }
    public string? DescricaoErro { get; private set; }
    public int? CodigoFicha { get; private set; }

    public bool EhPendente => Pendente == 1;

    protected LogSistema() { }

    public static LogSistema CriarErro(int codigoFilial, string descricaoErro, string? usuario, int? codigoFicha = null)
    {
        return new LogSistema
        {
            CodigoFilial = codigoFilial,
            TipoLog = 1,
            Pendente = 1,
            DataLog = DateTime.Now,
            Usuario = usuario,
            DescricaoErro = descricaoErro,
            CodigoFicha = codigoFicha
        };
    }

    public static LogSistema CriarInfo(int codigoFilial, string descricao, string? usuario, int? codigoFicha = null)
    {
        return new LogSistema
        {
            CodigoFilial = codigoFilial,
            TipoLog = 0,
            Pendente = 0,
            DataLog = DateTime.Now,
            Usuario = usuario,
            DescricaoErro = descricao,
            CodigoFicha = codigoFicha
        };
    }
}
