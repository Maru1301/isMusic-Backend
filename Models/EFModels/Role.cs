using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class Role
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("roleName")]
    [StringLength(50)]
    public string RoleName { get; set; } = null!;

    [InverseProperty("Role")]
    public virtual ICollection<AdminRoleMetadatum> AdminRoleMetadata { get; } = new List<AdminRoleMetadatum>();
}
