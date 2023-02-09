using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class AlbumType
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("typeName")]
    [StringLength(10)]
    public string TypeName { get; set; } = null!;

    [InverseProperty("AlbumType")]
    public virtual ICollection<Album> Albums { get; } = new List<Album>();
}
