using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

[Table("Activity_Tag_Metadata")]
public partial class ActivityTagMetadatum
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("activityId")]
    public int ActivityId { get; set; }

    [Column("tagId")]
    public int TagId { get; set; }

    [ForeignKey("ActivityId")]
    [InverseProperty("ActivityTagMetadata")]
    public virtual Activity Activity { get; set; } = null!;

    [ForeignKey("TagId")]
    [InverseProperty("ActivityTagMetadata")]
    public virtual ActivityTag Tag { get; set; } = null!;
}
