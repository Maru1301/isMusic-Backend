using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class ActivityType
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("typeName")]
    [StringLength(30)]
    public string TypeName { get; set; } = null!;

    [InverseProperty("ActivityType")]
    public virtual ICollection<Activity> Activities { get; } = new List<Activity>();
}
