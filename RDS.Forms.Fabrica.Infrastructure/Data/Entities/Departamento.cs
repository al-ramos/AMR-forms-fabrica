using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RDS.Forms.Fabrica.Infrastructure.Data.Entities;

[Table("DEPARTAMENTO", Schema = "rds")]
public partial class Departamento
{
    [Key]
    [Column("CD_DEPARTAMENTO")]
    public int CdDepartamento { get; set; }

    [Column("CD_FILIAL")]
    public int CdFilial { get; set; }

    [Column("DS_PRODUTO")]
    [StringLength(200)]
    [Unicode(false)]
    public string? DsProduto { get; set; }

    [ForeignKey("CdFilial")]
    [InverseProperty("Departamentos")]
    public virtual Filial CdFilialNavigation { get; set; } = null!;
}
