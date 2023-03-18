using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class ActivityFollow
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("activityId")]
    public int ActivityId { get; set; }

    [Column("memberId")]
    public int MemberId { get; set; }

    [Column("attendDate", TypeName = "datetime")]
    public DateTime AttendDate { get; set; }

    [ForeignKey("ActivityId")]
    [InverseProperty("ActivityFollows")]
    public virtual Activity Activity { get; set; } = null!;

    [ForeignKey("MemberId")]
    [InverseProperty("ActivityFollows")]
    public virtual Member Member { get; set; } = null!;
}
