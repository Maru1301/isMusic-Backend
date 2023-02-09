using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

[Table("Playlist_Song_Metadata")]
public partial class PlaylistSongMetadatum
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("playListId")]
    public int PlayListId { get; set; }

    [Column("songId")]
    public int SongId { get; set; }

    [Column("displayOrder")]
    public int DisplayOrder { get; set; }

    [Column("addedTime", TypeName = "date")]
    public DateTime AddedTime { get; set; }

    [ForeignKey("PlayListId")]
    [InverseProperty("PlaylistSongMetadata")]
    public virtual Playlist PlayList { get; set; } = null!;

    [ForeignKey("SongId")]
    [InverseProperty("PlaylistSongMetadata")]
    public virtual Song Song { get; set; } = null!;
}
