using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

[Table("Admin_Role_Metadata")]
public partial class AdminRoleMetadatum
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("adminId")]
    public int AdminId { get; set; }

    [Column("roleId")]
    public int RoleId { get; set; }

    [ForeignKey("AdminId")]
    [InverseProperty("AdminRoleMetadata")]
    public virtual Admin Admin { get; set; } = null!;

    [ForeignKey("RoleId")]
    [InverseProperty("AdminRoleMetadata")]
    public virtual Role Role { get; set; } = null!;
}
