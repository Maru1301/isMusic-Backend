using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class CartItem
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("cartId")]
    public int CartId { get; set; }

    [Column("productId")]
    public int ProductId { get; set; }

    [Column("qty")]
    public int Qty { get; set; }

    [ForeignKey("CartId")]
    [InverseProperty("CartItems")]
    public virtual Cart Cart { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("CartItems")]
    public virtual Product Product { get; set; } = null!;
}
