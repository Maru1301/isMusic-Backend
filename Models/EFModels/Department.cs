using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class Department
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("departmentName")]
    [StringLength(50)]
    public string DepartmentName { get; set; } = null!;

    [InverseProperty("Department")]
    public virtual ICollection<Admin> Admins { get; } = new List<Admin>();
}
