using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

[Table("Song_Creator_Metadata")]
public partial class SongCreatorMetadatum
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("songId")]
    public int SongId { get; set; }

    [Column("creatorId")]
    public int CreatorId { get; set; }

    [ForeignKey("CreatorId")]
    [InverseProperty("SongCreatorMetadata")]
    public virtual Creator Creator { get; set; } = null!;

    [ForeignKey("SongId")]
    [InverseProperty("SongCreatorMetadata")]
    public virtual Song Song { get; set; } = null!;
}
