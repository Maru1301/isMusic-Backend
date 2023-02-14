using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class Playlist
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("listName")]
    [StringLength(50)]
    public string ListName { get; set; } = null!;

    [Column("playlistCoverPath")]
    [StringLength(200)]
    public string? PlaylistCoverPath { get; set; }

    [Column("memberId")]
    public int MemberId { get; set; }

    [Column("description")]
    [StringLength(300)]
    public string? Description { get; set; }

    [Column("isPublic")]
    public bool IsPublic { get; set; }

    [Column("created", TypeName = "datetime")]
    public DateTime Created { get; set; }

    [InverseProperty("Playlist")]
    public virtual ICollection<LikedPlaylist> LikedPlaylists { get; } = new List<LikedPlaylist>();

    [ForeignKey("MemberId")]
    [InverseProperty("Playlists")]
    public virtual Member Member { get; set; } = null!;

    [InverseProperty("PlayList")]
    public virtual ICollection<PlaylistSongMetadatum> PlaylistSongMetadata { get; } = new List<PlaylistSongMetadatum>();

    [InverseProperty("Playlist")]
    public virtual ICollection<Queue> Queues { get; } = new List<Queue>();
}
