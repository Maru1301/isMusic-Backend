using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

[Index("AdminAccount", Name = "IX_Admins", IsUnique = true)]
public partial class Admin
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("adminAccount")]
    [StringLength(30)]
    [Unicode(false)]
    public string AdminAccount { get; set; } = null!;

    [Column("adminEncryptedPassword")]
    [StringLength(70)]
    [Unicode(false)]
    public string AdminEncryptedPassword { get; set; } = null!;

    [Column("departmentId")]
    public int DepartmentId { get; set; }

    [InverseProperty("CheckedBy")]
    public virtual ICollection<Activity> Activities { get; } = new List<Activity>();

    [InverseProperty("Admin")]
    public virtual ICollection<AdminRoleMetadatum> AdminRoleMetadata { get; } = new List<AdminRoleMetadatum>();

    [ForeignKey("DepartmentId")]
    [InverseProperty("Admins")]
    public virtual Department Department { get; set; } = null!;
}
