using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Merve.DataAccess
{
    [Table("roles")]
    public partial class Role
    {
        public Role()
        {
            RolePermissions = new HashSet<RolePermission>();
            UserRoles = new HashSet<UserRole>();
        }

        [Key]
        [Column("role_id", TypeName = "character varying")]
        public string RoleId { get; set; }
        [Column("description", TypeName = "character varying")]
        public string Description { get; set; }
        [Column("is_default")]
        public bool IsDefault { get; set; }
        [Column("created_at", TypeName = "timestamp with time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at", TypeName = "timestamp with time zone")]
        public DateTime UpdatedAt { get; set; }

        [InverseProperty(nameof(RolePermission.Role))]
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
        [InverseProperty(nameof(UserRole.Role))]
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
