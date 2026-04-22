using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RDS.Forms.Fabrica.Infrastructure.Data.Entities;

[Table("ESTACAO", Schema = "rds")]
public partial class Estacao
{
    [Key]
    [Column("CD_ESTACAO")]
    public int CdEstacao { get; set; }

    [Column("CD_FILIAL")]
    public int CdFilial { get; set; }

    [Column("NO_LOCAL")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NoLocal { get; set; }

    [ForeignKey("CdFilial")]
    [InverseProperty("Estacaos")]
    public virtual Filial CdFilialNavigation { get; set; } = null!;
}
