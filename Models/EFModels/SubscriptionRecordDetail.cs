using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class SubscriptionRecordDetail
{
    [Key]
    public int Id { get; set; }

    public int SubscriptionRecordId { get; set; }

    public int MemberId { get; set; }

    [ForeignKey("MemberId")]
    [InverseProperty("SubscriptionRecordDetails")]
    public virtual Member Member { get; set; } = null!;

    [ForeignKey("SubscriptionRecordId")]
    [InverseProperty("SubscriptionRecordDetails")]
    public virtual SubscriptionRecord SubscriptionRecord { get; set; } = null!;
}
