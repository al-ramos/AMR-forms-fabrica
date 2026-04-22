using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RDS.Forms.Fabrica.Infrastructure.Data.Entities;

[Keyless]
[Table("DEPARTAMENTO_PRODUTO", Schema = "rds")]
public partial class DepartamentoProduto
{
    [Column("CD_FILIAL")]
    public int CdFilial { get; set; }

    [Column("CD_PRODUTO")]
    public int CdProduto { get; set; }

    [Column("CD_DEPARTAMENTO")]
    public int CdDepartamento { get; set; }

    [ForeignKey("CdDepartamento")]
    public virtual Departamento CdDepartamentoNavigation { get; set; } = null!;

    [ForeignKey("CdFilial")]
    public virtual Filial CdFilialNavigation { get; set; } = null!;

    [ForeignKey("CdProduto")]
    public virtual Produto CdProdutoNavigation { get; set; } = null!;
}
