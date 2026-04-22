using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RDS.Forms.Fabrica.Infrastructure.Data.Entities;

[Keyless]
[Table("ADDRESS_BOOK_EXTENDED", Schema = "rds")]
public partial class AddressBookExtended
{
    [Column("CD_ADDRESS_NUMBER")]
    public int CdAddressNumber { get; set; }

    [Column("NO_NOME_CORRESPONDENCIA")]
    [StringLength(200)]
    [Unicode(false)]
    public string? NoNomeCorrespondencia { get; set; }

    [ForeignKey("CdAddressNumber")]
    public virtual AddressBook CdAddressNumberNavigation { get; set; } = null!;
}
