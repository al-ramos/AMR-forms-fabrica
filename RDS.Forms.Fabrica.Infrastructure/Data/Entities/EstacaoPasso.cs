using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RDS.Forms.Fabrica.Infrastructure.Data.Entities;

[Keyless]
[Table("ESTACAO_PASSO", Schema = "rds")]
public partial class EstacaoPasso
{
    [Column("CD_ESTACAO")]
    public int CdEstacao { get; set; }

    [Column("CD_FILIAL")]
    public int CdFilial { get; set; }

    [Column("CD_PASSO")]
    public int CdPasso { get; set; }

    [ForeignKey("CdEstacao")]
    public virtual Estacao CdEstacaoNavigation { get; set; } = null!;

    [ForeignKey("CdFilial")]
    public virtual Filial CdFilialNavigation { get; set; } = null!;

    [ForeignKey("CdPasso")]
    public virtual Passo CdPassoNavigation { get; set; } = null!;
}
