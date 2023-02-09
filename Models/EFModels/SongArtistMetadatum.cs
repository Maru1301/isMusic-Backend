using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

[Table("Song_Artist_Metadata")]
public partial class SongArtistMetadatum
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("songId")]
    public int SongId { get; set; }

    [Column("artistId")]
    public int ArtistId { get; set; }

    [ForeignKey("ArtistId")]
    [InverseProperty("SongArtistMetadata")]
    public virtual Artist Artist { get; set; } = null!;

    [ForeignKey("SongId")]
    [InverseProperty("SongArtistMetadata")]
    public virtual Song Song { get; set; } = null!;
}
