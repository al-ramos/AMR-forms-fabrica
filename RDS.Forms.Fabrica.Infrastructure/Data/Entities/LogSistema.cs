using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RDS.Forms.Fabrica.Infrastructure.Data.Entities;

[Keyless]
[Table("LOG_SISTEMA", Schema = "rds")]
[Index("CdFicha", Name = "IX_LOG_FICHA")]
[Index("CdFilial", "DtLog", Name = "IX_LOG_FILIAL_DT")]
public partial class LogSistema
{
    [Column("CD_FILIAL")]
    public int CdFilial { get; set; }

    [Column("CD_TIPOLOG")]
    public int? CdTipolog { get; set; }

    [Column("IC_PENDENTE")]
    public int? IcPendente { get; set; }

    [Column("DT_LOG", TypeName = "datetime")]
    public DateTime? DtLog { get; set; }

    [Column("CD_USUARIO")]
    [StringLength(50)]
    [Unicode(false)]
    public string? CdUsuario { get; set; }

    [Column("DS_ERRO")]
    [Unicode(false)]
    public string? DsErro { get; set; }

    [Column("CD_FICHA")]
    public int? CdFicha { get; set; }

    [ForeignKey("CdFicha")]
    public virtual Ficha? CdFichaNavigation { get; set; }

    [ForeignKey("CdFilial")]
    public virtual Filial CdFilialNavigation { get; set; } = null!;
}
