using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class Song
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("songName")]
    [StringLength(50)]
    public string SongName { get; set; } = null!;

    [Column("genreId")]
    public int GenreId { get; set; }

    [Column("duration")]
    public int Duration { get; set; }

    [Column("isInstrumental")]
    public bool IsInstrumental { get; set; }

    [Column("language")]
    [StringLength(50)]
    public string? Language { get; set; }

    [Column("isExplicit")]
    public bool? IsExplicit { get; set; }

    [Column("released", TypeName = "datetime")]
    public DateTime Released { get; set; }

    [Column("songWriter")]
    [StringLength(50)]
    public string SongWriter { get; set; } = null!;

    [Column("lyric")]
    [StringLength(2000)]
    public string? Lyric { get; set; }

    [Column("songCoverPath")]
    [StringLength(200)]
    public string SongCoverPath { get; set; } = null!;

    [Column("songPath")]
    [StringLength(200)]
    public string SongPath { get; set; } = null!;

    [Column("status")]
    public bool Status { get; set; }

    [Column("albumId")]
    public int? AlbumId { get; set; }

    [Column("displayOrderInAlbum")]
    public int? DisplayOrderInAlbum { get; set; }

    [ForeignKey("AlbumId")]
    [InverseProperty("Songs")]
    public virtual Album? Album { get; set; }

    [ForeignKey("GenreId")]
    [InverseProperty("Songs")]
    public virtual SongGenre Genre { get; set; } = null!;

    [InverseProperty("Song")]
    public virtual ICollection<LikedSong> LikedSongs { get; } = new List<LikedSong>();

    [InverseProperty("Song")]
    public virtual ICollection<PlaylistSongMetadatum> PlaylistSongMetadata { get; } = new List<PlaylistSongMetadatum>();

    [InverseProperty("Song")]
    public virtual ICollection<QueueSong> QueueSongs { get; } = new List<QueueSong>();

    [InverseProperty("Song")]
    public virtual ICollection<SongArtistMetadatum> SongArtistMetadata { get; } = new List<SongArtistMetadatum>();

    [InverseProperty("Song")]
    public virtual ICollection<SongCreatorMetadatum> SongCreatorMetadata { get; } = new List<SongCreatorMetadatum>();

    [InverseProperty("Song")]
    public virtual ICollection<SongPlayedRecord> SongPlayedRecords { get; } = new List<SongPlayedRecord>();
}
