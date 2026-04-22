using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RDS.Forms.Fabrica.Infrastructure.Data.Entities;

[Table("PRODUTO", Schema = "rds")]
[Index("CdBusinessUnit", Name = "IX_PRODUTO_BUSINESS_UNIT")]
public partial class Produto
{
    [Key]
    [Column("CD_PRODUTO")]
    public int CdProduto { get; set; }

    [Column("CD_BUSINESS_UNIT")]
    [StringLength(20)]
    [Unicode(false)]
    public string? CdBusinessUnit { get; set; }

    [Column("CD_PRODUTO_LONGO")]
    [StringLength(50)]
    [Unicode(false)]
    public string? CdProdutoLongo { get; set; }

    [Column("NO_PRODUTO")]
    [StringLength(200)]
    [Unicode(false)]
    public string? NoProduto { get; set; }

    [Column("CD_EAN")]
    [StringLength(30)]
    [Unicode(false)]
    public string? CdEan { get; set; }

    [Column("CD_UMC")]
    [StringLength(10)]
    [Unicode(false)]
    public string? CdUmc { get; set; }

    [Column("CD_UM")]
    [StringLength(10)]
    [Unicode(false)]
    public string? CdUm { get; set; }

    [Column("CD_CTF")]
    [StringLength(30)]
    [Unicode(false)]
    public string? CdCtf { get; set; }

    [Column("CD_CLF")]
    [StringLength(30)]
    [Unicode(false)]
    public string? CdClf { get; set; }

    [ForeignKey("CdBusinessUnit")]
    [InverseProperty("Produtos")]
    public virtual BusinessUnit? CdBusinessUnitNavigation { get; set; }
}
