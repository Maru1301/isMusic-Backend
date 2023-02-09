using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class Order
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("memberId")]
    public int MemberId { get; set; }

    [Column("couponId")]
    public int CouponId { get; set; }

    [Column("payments")]
    public int Payments { get; set; }

    [Column("orderStatus")]
    public bool OrderStatus { get; set; }

    [Column("paid")]
    public bool Paid { get; set; }

    [Column("created", TypeName = "datetime")]
    public DateTime Created { get; set; }

    [Column("receiver")]
    [StringLength(30)]
    public string Receiver { get; set; } = null!;

    [Column("address")]
    [StringLength(200)]
    public string Address { get; set; } = null!;

    [Column("cellphone")]
    [StringLength(10)]
    [Unicode(false)]
    public string Cellphone { get; set; } = null!;

    [ForeignKey("CouponId")]
    [InverseProperty("Orders")]
    public virtual Coupon Coupon { get; set; } = null!;

    [ForeignKey("MemberId")]
    [InverseProperty("Orders")]
    public virtual Member Member { get; set; } = null!;

    [InverseProperty("Order")]
    public virtual ICollection<OrderProductMetadatum> OrderProductMetadata { get; } = new List<OrderProductMetadatum>();
}
