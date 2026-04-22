using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RDS.Forms.Fabrica.Infrastructure.Data.Entities;

[Table("BUSINESS_UNIT", Schema = "rds")]
public partial class BusinessUnit
{
    [Key]
    [Column("CD_BUSINESS_UNIT")]
    [StringLength(20)]
    [Unicode(false)]
    public string CdBusinessUnit { get; set; } = null!;

    [Column("NO_BUSINESS_UNIT")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NoBusinessUnit { get; set; }

    [Column("CD_COMPANHIA")]
    [StringLength(20)]
    [Unicode(false)]
    public string? CdCompanhia { get; set; }

    [Column("CD_ADDRESS_NUMBER")]
    public int? CdAddressNumber { get; set; }

    [ForeignKey("CdAddressNumber")]
    [InverseProperty("BusinessUnits")]
    public virtual AddressBook? CdAddressNumberNavigation { get; set; }

    [InverseProperty("CdBusinessUnitNavigation")]
    public virtual ICollection<Ficha> Fichas { get; set; } = new List<Ficha>();

    [InverseProperty("CdBusinessUnitNavigation")]
    public virtual ICollection<NotaFiscal> NotaFiscals { get; set; } = new List<NotaFiscal>();

    [InverseProperty("CdBusinessUnitNavigation")]
    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    [InverseProperty("CdBusinessUnitNavigation")]
    public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}
