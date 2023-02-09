using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class Avatar
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("path")]
    [StringLength(50)]
    public string Path { get; set; } = null!;

    [InverseProperty("Avatar")]
    public virtual ICollection<Member> Members { get; } = new List<Member>();
}
