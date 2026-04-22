using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RDS.Forms.Fabrica.Infrastructure.Data.Entities;

[Table("PEDIDO", Schema = "rds")]
public partial class Pedido
{
    [Key]
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

    [Column("DT_PEDIDO")]
    public DateOnly? DtPedido { get; set; }

    [Column("CD_TIPO_DOCTO_JDE")]
    [StringLength(10)]
    [Unicode(false)]
    public string? CdTipoDoctoJde { get; set; }

    [ForeignKey("CdAddressNumber")]
    [InverseProperty("Pedidos")]
    public virtual AddressBook? CdAddressNumberNavigation { get; set; }

    [ForeignKey("CdBusinessUnit")]
    [InverseProperty("Pedidos")]
    public virtual BusinessUnit? CdBusinessUnitNavigation { get; set; }

    [ForeignKey("CdFilial")]
    [InverseProperty("Pedidos")]
    public virtual Filial CdFilialNavigation { get; set; } = null!;
}
