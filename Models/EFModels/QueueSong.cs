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

    [Column("nextQueueSong")]
    public int? NextQueueSong { get; set; }

    [Column("fromAlbumOrPlaylist")]
    public bool FromAlbumOrPlaylist { get; set; }

    [ForeignKey("QueueId")]
    [InverseProperty("QueueSongs")]
    public virtual Queue Queue { get; set; } = null!;
}
