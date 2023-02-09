using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class Cart
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("memberId")]
    public int MemberId { get; set; }

    [InverseProperty("Cart")]
    public virtual ICollection<CartItem> CartItems { get; } = new List<CartItem>();

    [ForeignKey("MemberId")]
    [InverseProperty("Carts")]
    public virtual Member Member { get; set; } = null!;
}
