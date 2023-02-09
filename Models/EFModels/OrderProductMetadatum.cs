using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

[Table("Order_Product_Metadata")]
public partial class OrderProductMetadatum
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("orderId")]
    public int OrderId { get; set; }

    [Column("productId")]
    public int ProductId { get; set; }

    [Column("price", TypeName = "decimal(10, 0)")]
    public decimal Price { get; set; }

    [Column("productName")]
    [StringLength(50)]
    public string ProductName { get; set; } = null!;

    [Column("qty")]
    public int Qty { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderProductMetadata")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("OrderProductMetadata")]
    public virtual Product Product { get; set; } = null!;
}
