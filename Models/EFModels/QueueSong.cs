using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class QueueSong
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("queueId")]
    public int QueueId { get; set; }

    [Column("songId")]
    public int SongId { get; set; }

    [Column("displayOrder")]
    public int DisplayOrder { get; set; }

    [Column("albumId")]
    public int AlbumId { get; set; }

    [Column("playlistId")]
    public int PlaylistId { get; set; }

    [ForeignKey("AlbumId")]
    [InverseProperty("QueueSongs")]
    public virtual Album Album { get; set; } = null!;

    [ForeignKey("PlaylistId")]
    [InverseProperty("QueueSongs")]
    public virtual Playlist Playlist { get; set; } = null!;

    [ForeignKey("QueueId")]
    [InverseProperty("QueueSongs")]
    public virtual Queue Queue { get; set; } = null!;
}
