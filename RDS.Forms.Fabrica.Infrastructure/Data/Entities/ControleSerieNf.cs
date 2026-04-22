using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RDS.Forms.Fabrica.Infrastructure.Data.Entities;

[Table("CONTROLE_SERIE_NF", Schema = "rds")]
public partial class ControleSerieNf
{
    [Key]
    [Column("CD_SER_NOTA_FISCAL")]
    [StringLength(10)]
    [Unicode(false)]
    public string CdSerNotaFiscal { get; set; } = null!;

    [Column("CD_FILIAL")]
    public int CdFilial { get; set; }

    [Column("CD_NOTA_FISCAL")]
    public int? CdNotaFiscal { get; set; }

    [Column("DT_ULTIMA_ATUALIZACAO")]
    public DateOnly? DtUltimaAtualizacao { get; set; }

    [Column("CD_USUARIO_ATUALIZADOR")]
    [StringLength(50)]
    [Unicode(false)]
    public string? CdUsuarioAtualizador { get; set; }

    [ForeignKey("CdFilial")]
    [InverseProperty("ControleSerieNfs")]
    public virtual Filial CdFilialNavigation { get; set; } = null!;
}
