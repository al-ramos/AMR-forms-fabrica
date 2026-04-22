using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RDS.Forms.Fabrica.Infrastructure.Data.Entities;

[Keyless]
[Table("TIPO_OPERACAO_PASSO", Schema = "rds")]
public partial class TipoOperacaoPasso
{
    [Column("CD_FILIAL")]
    public int CdFilial { get; set; }

    [Column("CD_TIPO_OPERACAO")]
    public int CdTipoOperacao { get; set; }

    [Column("CD_PASSO")]
    public int CdPasso { get; set; }

    [Column("VL_SEQUENCIA")]
    public int? VlSequencia { get; set; }

    [Column("CD_TIPO_PASSO")]
    public int? CdTipoPasso { get; set; }

    [Column("CD_PASSO_FLUTUANTE")]
    public int? CdPassoFlutuante { get; set; }

    [Column("CD_PASSO_RETORNO")]
    public int? CdPassoRetorno { get; set; }

    [ForeignKey("CdFilial")]
    public virtual Filial CdFilialNavigation { get; set; } = null!;

    [ForeignKey("CdPasso")]
    public virtual Passo CdPassoNavigation { get; set; } = null!;

    [ForeignKey("CdTipoOperacao")]
    public virtual TipoOperacao CdTipoOperacaoNavigation { get; set; } = null!;
}
