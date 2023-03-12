using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class Queue
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("memberId")]
    public int MemberId { get; set; }

    [Column("currentSongOrder")]
    public int CurrentSongOrder { get; set; }

    [Column("currentSongTime")]
    public int? CurrentSongTime { get; set; }

    [Column("isShuffle")]
    public bool IsShuffle { get; set; }

    [Column("isRepeat")]
    public bool? IsRepeat { get; set; }

    [Column("albumId")]
    public int? AlbumId { get; set; }

    [Column("playlistId")]
    public int? PlaylistId { get; set; }

    [Column("artistId")]
    public int? ArtistId { get; set; }

    [Column("inList")]
    public bool InList { get; set; }

    [ForeignKey("AlbumId")]
    [InverseProperty("Queues")]
    public virtual Album? Album { get; set; }

    [ForeignKey("ArtistId")]
    [InverseProperty("Queues")]
    public virtual Artist? Artist { get; set; }

    [ForeignKey("MemberId")]
    [InverseProperty("Queues")]
    public virtual Member Member { get; set; } = null!;

    [ForeignKey("PlaylistId")]
    [InverseProperty("Queues")]
    public virtual Playlist? Playlist { get; set; }

    [InverseProperty("Queue")]
    public virtual ICollection<QueueSong> QueueSongs { get; } = new List<QueueSong>();
}
