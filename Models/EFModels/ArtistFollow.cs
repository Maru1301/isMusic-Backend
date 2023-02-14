using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class ArtistFollow
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("artistId")]
    public int ArtistId { get; set; }

    [Column("memberId")]
    public int MemberId { get; set; }

    [Column("created", TypeName = "datetime")]
    public DateTime Created { get; set; }

    [ForeignKey("ArtistId")]
    [InverseProperty("ArtistFollows")]
    public virtual Artist Artist { get; set; } = null!;

    [ForeignKey("MemberId")]
    [InverseProperty("ArtistFollows")]
    public virtual Member Member { get; set; } = null!;
}
