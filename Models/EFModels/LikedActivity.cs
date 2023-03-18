using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class LikedActivity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("memberId")]
    public int MemberId { get; set; }

    [Column("activityId")]
    public int ActivityId { get; set; }

    [Column("created", TypeName = "datetime")]
    public DateTime Created { get; set; }

    [ForeignKey("ActivityId")]
    [InverseProperty("LikedActivities")]
    public virtual Activity Activity { get; set; } = null!;

    [ForeignKey("MemberId")]
    [InverseProperty("LikedActivities")]
    public virtual Member Member { get; set; } = null!;
}
