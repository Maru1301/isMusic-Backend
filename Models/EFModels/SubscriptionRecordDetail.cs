using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

[Keyless]
public partial class SubscriptionRecordDetail
{
    [Column("id")]
    public int Id { get; set; }

    [Column("subscriptionRecordId")]
    public int SubscriptionRecordId { get; set; }

    [Column("memberId")]
    public int MemberId { get; set; }
}
