using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

[Index("CreatorName", Name = "UK_Creators", IsUnique = true)]
public partial class Creator
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("creatorName")]
    [StringLength(50)]
    public string CreatorName { get; set; } = null!;

    [Column("creatorGender")]
    public byte? CreatorGender { get; set; }

    [Column("creatorAbout")]
    [StringLength(500)]
    public string? CreatorAbout { get; set; }

    [Column("memberId")]
    public int MemberId { get; set; }

    [Column("creatorPicPath")]
    [StringLength(100)]
    public string? CreatorPicPath { get; set; }

    [Column("creatorCoverPath")]
    [StringLength(100)]
    public string? CreatorCoverPath { get; set; }

    [InverseProperty("MainCreator")]
    public virtual ICollection<Album> Albums { get; } = new List<Album>();

    [InverseProperty("Creator")]
    public virtual ICollection<CreatorFollow> CreatorFollows { get; } = new List<CreatorFollow>();

    [ForeignKey("MemberId")]
    [InverseProperty("Creators")]
    public virtual Member Member { get; set; } = null!;

    [InverseProperty("Creator")]
    public virtual ICollection<SongCreatorMetadatum> SongCreatorMetadata { get; } = new List<SongCreatorMetadatum>();
}
