using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class Coupon
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("couponText")]
    [StringLength(50)]
    public string CouponText { get; set; } = null!;

    [Column("startDate", TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [Column("expiredDate", TypeName = "datetime")]
    public DateTime ExpiredDate { get; set; }

    [Column("discounts")]
    [StringLength(20)]
    public string Discounts { get; set; } = null!;

    [InverseProperty("Coupon")]
    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
