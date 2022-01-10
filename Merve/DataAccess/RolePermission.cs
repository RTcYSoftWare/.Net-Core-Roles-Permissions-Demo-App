using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Merve.DataAccess
{
    [Table("role_permissions")]
    [Index(nameof(PermissionId), nameof(RoleId), Name = "role_permissions_permission_id_role_id_key", IsUnique = true)]
    public partial class RolePermission
    {
        [Key]
        [Column("role_permission_id")]
        public int RolePermissionId { get; set; }
        [Required]
        [Column("permission_id", TypeName = "character varying")]
        public string PermissionId { get; set; }
        [Required]
        [Column("role_id", TypeName = "character varying")]
        public string RoleId { get; set; }
        [Column("created_at", TypeName = "timestamp with time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at", TypeName = "timestamp with time zone")]
        public DateTime UpdatedAt { get; set; }

        [ForeignKey(nameof(PermissionId))]
        [InverseProperty("RolePermissions")]
        public virtual Permission Permission { get; set; }
        [ForeignKey(nameof(RoleId))]
        [InverseProperty("RolePermissions")]
        public virtual Role Role { get; set; }
    }
}
