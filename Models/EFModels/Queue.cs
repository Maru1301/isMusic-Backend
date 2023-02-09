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

    [Column("currentSongId")]
    public int? CurrentSongId { get; set; }

    [Column("currentSongTime")]
    public int? CurrentSongTime { get; set; }

    [Column("isShuffle")]
    public bool IsShuffle { get; set; }

    [Column("isRepeat")]
    public bool? IsRepeat { get; set; }

    [ForeignKey("CurrentSongId")]
    [InverseProperty("Queues")]
    public virtual Song? CurrentSong { get; set; }

    [ForeignKey("MemberId")]
    [InverseProperty("Queues")]
    public virtual Member Member { get; set; } = null!;

    [InverseProperty("Queue")]
    public virtual ICollection<QueueSong> QueueSongs { get; } = new List<QueueSong>();
}
