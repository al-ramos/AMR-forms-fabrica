using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RDS.Forms.Fabrica.Infrastructure.Data.Entities;

[Keyless]
[Table("PEDIDO_ITEM", Schema = "rds")]
public partial class PedidoItem
{
    [Column("CD_PEDIDO")]
    public int CdPedido { get; set; }

    [Column("CD_FILIAL")]
    public int CdFilial { get; set; }

    [Column("CD_BUSINESS_UNIT")]
    [StringLength(20)]
    [Unicode(false)]
    public string? CdBusinessUnit { get; set; }

    [Column("CD_ADDRESS_NUMBER")]
    public int? CdAddressNumber { get; set; }

    [Column("CD_PRODUTO")]
    public int? CdProduto { get; set; }

    [Column("QT_PRODUTO", TypeName = "decimal(18, 4)")]
    public decimal? QtProduto { get; set; }

    [Column("CD_UM")]
    [StringLength(10)]
    [Unicode(false)]
    public string? CdUm { get; set; }

    [Column("CD_TIPO_DOCTO_JDE")]
    [StringLength(10)]
    [Unicode(false)]
    public string? CdTipoDoctoJde { get; set; }

    [ForeignKey("CdAddressNumber")]
    public virtual AddressBook? CdAddressNumberNavigation { get; set; }

    [ForeignKey("CdBusinessUnit")]
    public virtual BusinessUnit? CdBusinessUnitNavigation { get; set; }

    [ForeignKey("CdFilial")]
    public virtual Filial CdFilialNavigation { get; set; } = null!;

    [ForeignKey("CdPedido")]
    public virtual Pedido CdPedidoNavigation { get; set; } = null!;

    [ForeignKey("CdProduto")]
    public virtual Produto? CdProdutoNavigation { get; set; }
}
