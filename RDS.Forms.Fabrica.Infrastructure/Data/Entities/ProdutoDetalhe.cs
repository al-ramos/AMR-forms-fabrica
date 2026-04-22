using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RDS.Forms.Fabrica.Infrastructure.Data.Entities;

[Keyless]
[Table("PRODUTO_DETALHE", Schema = "rds")]
public partial class ProdutoDetalhe
{
    [Column("CD_PRODUTO")]
    public int CdProduto { get; set; }

    [Column("CD_BUSINESS_UNIT")]
    [StringLength(20)]
    [Unicode(false)]
    public string CdBusinessUnit { get; set; } = null!;

    [Column("CD_UMC")]
    [StringLength(10)]
    [Unicode(false)]
    public string? CdUmc { get; set; }

    [ForeignKey("CdBusinessUnit")]
    public virtual BusinessUnit CdBusinessUnitNavigation { get; set; } = null!;

    [ForeignKey("CdProduto")]
    public virtual Produto CdProdutoNavigation { get; set; } = null!;
}
