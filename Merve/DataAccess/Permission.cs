using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Merve.DataAccess
{
    [Table("permissions")]
    public partial class Permission
    {
        public Permission()
        {
            RolePermissions = new HashSet<RolePermission>();
        }

        [Key]
        [Column("permission_id", TypeName = "character varying")]
        public string PermissionId { get; set; }
        [Column("description", TypeName = "character varying")]
        public string Description { get; set; }

        [InverseProperty(nameof(RolePermission.Permission))]
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}
