using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class SubscriptionPlan
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("planName")]
    [StringLength(50)]
    public string PlanName { get; set; } = null!;

    [Column("price", TypeName = "decimal(7, 0)")]
    public decimal Price { get; set; }

    [Column("numberOfUsers")]
    public byte NumberOfUsers { get; set; }

    [Column("description")]
    [StringLength(500)]
    public string Description { get; set; } = null!;

    [InverseProperty("SubscriptionPlan")]
    public virtual ICollection<SubscriptionRecord> SubscriptionRecords { get; } = new List<SubscriptionRecord>();
}
