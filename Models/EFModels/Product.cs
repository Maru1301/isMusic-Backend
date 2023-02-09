using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class Product
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("productCategoryId")]
    public int ProductCategoryId { get; set; }

    [Column("productPrice", TypeName = "decimal(7, 0)")]
    public decimal ProductPrice { get; set; }

    [Column("albumId")]
    public int AlbumId { get; set; }

    [Column("productName")]
    [StringLength(50)]
    public string ProductName { get; set; } = null!;

    [Column("stock")]
    public int Stock { get; set; }

    [Column("status")]
    public bool Status { get; set; }

    [ForeignKey("AlbumId")]
    [InverseProperty("Products")]
    public virtual Album Album { get; set; } = null!;

    [InverseProperty("Product")]
    public virtual ICollection<CartItem> CartItems { get; } = new List<CartItem>();

    [InverseProperty("Product")]
    public virtual ICollection<OrderProductMetadatum> OrderProductMetadata { get; } = new List<OrderProductMetadatum>();

    [ForeignKey("ProductCategoryId")]
    [InverseProperty("Products")]
    public virtual ProductCategory ProductCategory { get; set; } = null!;
}
