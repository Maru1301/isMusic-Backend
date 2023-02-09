using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class SongPlayedRecord
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("songId")]
    public int SongId { get; set; }

    [Column("memberId")]
    public int MemberId { get; set; }

    [Column("playedDate", TypeName = "datetime")]
    public DateTime PlayedDate { get; set; }

    [ForeignKey("MemberId")]
    [InverseProperty("SongPlayedRecords")]
    public virtual Member Member { get; set; } = null!;

    [ForeignKey("SongId")]
    [InverseProperty("SongPlayedRecords")]
    public virtual Song Song { get; set; } = null!;
}
