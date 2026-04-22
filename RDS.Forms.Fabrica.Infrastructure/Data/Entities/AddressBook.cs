using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RDS.Forms.Fabrica.Infrastructure.Data.Entities;

[Table("ADDRESS_BOOK", Schema = "rds")]
public partial class AddressBook
{
    [Key]
    [Column("CD_ADDRESS_NUMBER")]
    public int CdAddressNumber { get; set; }

    [Column("CD_CNPJ")]
    [StringLength(20)]
    [Unicode(false)]
    public string? CdCnpj { get; set; }

    [Column("CD_INSCRICAO")]
    [StringLength(30)]
    [Unicode(false)]
    public string? CdInscricao { get; set; }

    [Column("ED_FILIAL")]
    [StringLength(200)]
    [Unicode(false)]
    public string? EdFilial { get; set; }

    [Column("NO_CIDADE")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NoCidade { get; set; }

    [Column("CD_UF")]
    [StringLength(2)]
    [Unicode(false)]
    public string? CdUf { get; set; }

    [Column("CD_CEP")]
    [StringLength(10)]
    [Unicode(false)]
    public string? CdCep { get; set; }

    [Column("NO_NOME")]
    [StringLength(200)]
    [Unicode(false)]
    public string? NoNome { get; set; }

    [Column("CD_TIPO_ADDRESS")]
    public int? CdTipoAddress { get; set; }

    [Column("IC_TRANSPORTADORA")]
    public int? IcTransportadora { get; set; }

    [InverseProperty("CdAddressNumberNavigation")]
    public virtual ICollection<BusinessUnit> BusinessUnits { get; set; } = new List<BusinessUnit>();

    [InverseProperty("CdAddressNumberTraNavigation")]
    public virtual ICollection<Ficha> Fichas { get; set; } = new List<Ficha>();

    [InverseProperty("CdAddressNumberNavigation")]
    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
