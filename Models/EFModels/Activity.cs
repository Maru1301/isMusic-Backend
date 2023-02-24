using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class Activity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("activityName")]
    [StringLength(30)]
    public string ActivityName { get; set; } = null!;

    [Column("activityStartTime", TypeName = "datetime")]
    public DateTime ActivityStartTime { get; set; }

    [Column("activityEndTime", TypeName = "datetime")]
    public DateTime ActivityEndTime { get; set; }

    [Column("activityLocation")]
    [StringLength(100)]
    public string ActivityLocation { get; set; } = null!;

    [Column("activityTypeId")]
    public int ActivityTypeId { get; set; }

    [Column("activityInfo")]
    [StringLength(4000)]
    public string ActivityInfo { get; set; } = null!;

    [Column("activityOrganizerId")]
    public int ActivityOrganizerId { get; set; }

    [Column("activityImagePath")]
    [StringLength(50)]
    [Unicode(false)]
    public string ActivityImagePath { get; set; } = null!;

    [Column("publishedStatus")]
    public bool PublishedStatus { get; set; }

    [Column("checkedById")]
    public int? CheckedById { get; set; }

    [Column("updated", TypeName = "datetime")]
    public DateTime Updated { get; set; }

    [InverseProperty("Activity")]
    public virtual ICollection<ActivityFollow> ActivityFollows { get; } = new List<ActivityFollow>();

    [ForeignKey("ActivityOrganizerId")]
    [InverseProperty("Activities")]
    public virtual Member ActivityOrganizer { get; set; } = null!;

    [InverseProperty("Activity")]
    public virtual ICollection<ActivityTagMetadatum> ActivityTagMetadata { get; } = new List<ActivityTagMetadatum>();

    [ForeignKey("ActivityTypeId")]
    [InverseProperty("Activities")]
    public virtual ActivityType ActivityType { get; set; } = null!;

    [ForeignKey("CheckedById")]
    [InverseProperty("Activities")]
    public virtual Admin? CheckedBy { get; set; }
}
