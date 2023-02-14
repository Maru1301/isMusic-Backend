using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class LikedPlaylist
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("playlistId")]
    public int PlaylistId { get; set; }

    [Column("memberId")]
    public int MemberId { get; set; }

    [Column("created", TypeName = "datetime")]
    public DateTime Created { get; set; }

    [ForeignKey("MemberId")]
    [InverseProperty("LikedPlaylists")]
    public virtual Member Member { get; set; } = null!;

    [ForeignKey("PlaylistId")]
    [InverseProperty("LikedPlaylists")]
    public virtual Playlist Playlist { get; set; } = null!;
}
