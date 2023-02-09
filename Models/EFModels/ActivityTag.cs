using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class ActivityTag
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("tagName")]
    [StringLength(50)]
    public string TagName { get; set; } = null!;

    [InverseProperty("Tag")]
    public virtual ICollection<ActivityTagMetadatum> ActivityTagMetadata { get; } = new List<ActivityTagMetadatum>();
}
