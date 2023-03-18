using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class SubscriptionRecord
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("memberId")]
    public int MemberId { get; set; }

    [Column("subscriptionPlanId")]
    public int SubscriptionPlanId { get; set; }

    [Column("subscribedTime", TypeName = "datetime")]
    public DateTime SubscribedTime { get; set; }

    [Column("subscribedExpireTime", TypeName = "datetime")]
    public DateTime SubscribedExpireTime { get; set; }

    [ForeignKey("MemberId")]
    [InverseProperty("SubscriptionRecords")]
    public virtual Member Member { get; set; } = null!;

    [ForeignKey("SubscriptionPlanId")]
    [InverseProperty("SubscriptionRecords")]
    public virtual SubscriptionPlan SubscriptionPlan { get; set; } = null!;

    [InverseProperty("SubscriptionRecord")]
    public virtual ICollection<SubscriptionRecordDetail> SubscriptionRecordDetails { get; } = new List<SubscriptionRecordDetail>();
}
