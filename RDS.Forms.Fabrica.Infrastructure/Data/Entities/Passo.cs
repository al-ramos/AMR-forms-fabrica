using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RDS.Forms.Fabrica.Infrastructure.Data.Entities;

[Table("PASSO", Schema = "rds")]
public partial class Passo
{
    [Key]
    [Column("CD_PASSO")]
    public int CdPasso { get; set; }

    [Column("NO_PASSO")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NoPasso { get; set; }

    [InverseProperty("CdPassoAtualNavigation")]
    public virtual ICollection<Ficha> Fichas { get; set; } = new List<Ficha>();
}
