using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class SongGenre
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("genreName")]
    [StringLength(50)]
    public string GenreName { get; set; } = null!;

    [InverseProperty("AlbumGenre")]
    public virtual ICollection<Album> Albums { get; } = new List<Album>();

    [InverseProperty("Genre")]
    public virtual ICollection<Song> Songs { get; } = new List<Song>();
}
