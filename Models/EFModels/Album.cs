using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class Album
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("albumName")]
    [StringLength(50)]
    public string AlbumName { get; set; } = null!;

    [Column("albumCoverPath")]
    [StringLength(200)]
    public string AlbumCoverPath { get; set; } = null!;

    [Column("albumTypeId")]
    public int AlbumTypeId { get; set; }

    [Column("albumGenreId")]
    public int AlbumGenreId { get; set; }

    [Column("released", TypeName = "date")]
    public DateTime Released { get; set; }

    [Column("description")]
    [StringLength(3000)]
    public string Description { get; set; } = null!;

    [Column("mainArtistId")]
    public int? MainArtistId { get; set; }

    [Column("mainCreatorId")]
    public int? MainCreatorId { get; set; }

    [Column("albumProducer")]
    [StringLength(50)]
    public string? AlbumProducer { get; set; }

    [Column("albumCompany")]
    [StringLength(50)]
    public string? AlbumCompany { get; set; }

    [ForeignKey("AlbumGenreId")]
    [InverseProperty("Albums")]
    public virtual SongGenre AlbumGenre { get; set; } = null!;

    [ForeignKey("AlbumTypeId")]
    [InverseProperty("Albums")]
    public virtual AlbumType AlbumType { get; set; } = null!;

    [InverseProperty("Album")]
    public virtual ICollection<LikedAlbum> LikedAlbums { get; } = new List<LikedAlbum>();

    [ForeignKey("MainArtistId")]
    [InverseProperty("Albums")]
    public virtual Artist? MainArtist { get; set; }

    [ForeignKey("MainCreatorId")]
    [InverseProperty("Albums")]
    public virtual Creator? MainCreator { get; set; }

    [InverseProperty("Album")]
    public virtual ICollection<Product> Products { get; } = new List<Product>();

    [InverseProperty("Album")]
    public virtual ICollection<Queue> Queues { get; } = new List<Queue>();

    [InverseProperty("Album")]
    public virtual ICollection<Song> Songs { get; } = new List<Song>();
}
