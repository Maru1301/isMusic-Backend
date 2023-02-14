using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class LikedAlbum
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("albumId")]
    public int AlbumId { get; set; }

    [Column("memberId")]
    public int MemberId { get; set; }

    [Column("created", TypeName = "datetime")]
    public DateTime Created { get; set; }

    [ForeignKey("AlbumId")]
    [InverseProperty("LikedAlbums")]
    public virtual Album Album { get; set; } = null!;

    [ForeignKey("MemberId")]
    [InverseProperty("LikedAlbums")]
    public virtual Member Member { get; set; } = null!;
}
