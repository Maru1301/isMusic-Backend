using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class ProductCategory
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("categoryName")]
    [StringLength(50)]
    public string CategoryName { get; set; } = null!;

    [InverseProperty("ProductCategory")]
    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
