using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class LikedSong
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("songId")]
    public int SongId { get; set; }

    [Column("memberId")]
    public int MemberId { get; set; }

    [Column("created", TypeName = "datetime")]
    public DateTime Created { get; set; }

    [ForeignKey("MemberId")]
    [InverseProperty("LikedSongs")]
    public virtual Member Member { get; set; } = null!;

    [ForeignKey("SongId")]
    [InverseProperty("LikedSongs")]
    public virtual Song Song { get; set; } = null!;
}
