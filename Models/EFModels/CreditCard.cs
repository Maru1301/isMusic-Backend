using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class CreditCard
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("creditCardNumber")]
    public int CreditCardNumber { get; set; }

    [Column("creditCardExpiredDate", TypeName = "date")]
    public DateTime CreditCardExpiredDate { get; set; }

    [Column("creditCardHolderName")]
    [StringLength(50)]
    public string CreditCardHolderName { get; set; } = null!;

    [InverseProperty("CreditCard")]
    public virtual ICollection<Member> Members { get; } = new List<Member>();
}
