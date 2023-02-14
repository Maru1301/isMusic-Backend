using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class Artist
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("artistName")]
    [StringLength(50)]
    public string ArtistName { get; set; } = null!;

    [Column("isBand")]
    public bool IsBand { get; set; }

    [Column("artistGender")]
    public bool? ArtistGender { get; set; }

    [Column("artistAbout")]
    [StringLength(500)]
    public string ArtistAbout { get; set; } = null!;

    [Column("artistPicPath")]
    [StringLength(100)]
    public string ArtistPicPath { get; set; } = null!;

    [InverseProperty("MainArtist")]
    public virtual ICollection<Album> Albums { get; } = new List<Album>();

    [InverseProperty("Artist")]
    public virtual ICollection<ArtistFollow> ArtistFollows { get; } = new List<ArtistFollow>();

    [InverseProperty("Artist")]
    public virtual ICollection<Queue> Queues { get; } = new List<Queue>();

    [InverseProperty("Artist")]
    public virtual ICollection<SongArtistMetadatum> SongArtistMetadata { get; } = new List<SongArtistMetadatum>();
}
