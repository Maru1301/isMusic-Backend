using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class CreatorFollow
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("creatorId")]
    public int CreatorId { get; set; }

    [Column("memberId")]
    public int MemberId { get; set; }

    [Column("created", TypeName = "datetime")]
    public DateTime Created { get; set; }

    [ForeignKey("CreatorId")]
    [InverseProperty("CreatorFollows")]
    public virtual Creator Creator { get; set; } = null!;

    [ForeignKey("MemberId")]
    [InverseProperty("CreatorFollows")]
    public virtual Member Member { get; set; } = null!;
}
