using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RDS.Forms.Fabrica.Infrastructure.Data.Entities;

[Keyless]
[Table("TIPO_OPERACAO_PASSO_CFG", Schema = "rds")]
public partial class TipoOperacaoPassoCfg
{
    [Column("CD_FILIAL")]
    public int CdFilial { get; set; }

    [Column("CD_TIPO_OPERACAO")]
    public int CdTipoOperacao { get; set; }

    [Column("NO_TABELA")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NoTabela { get; set; }

    [Column("NO_CAMPO")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NoCampo { get; set; }

    [Column("CD_TIPO")]
    [StringLength(1)]
    [Unicode(false)]
    public string? CdTipo { get; set; }

    [Column("IC_ENABLED")]
    public int? IcEnabled { get; set; }

    [ForeignKey("CdFilial")]
    public virtual Filial CdFilialNavigation { get; set; } = null!;

    [ForeignKey("CdTipoOperacao")]
    public virtual TipoOperacao CdTipoOperacaoNavigation { get; set; } = null!;
}
