using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Merve.DataAccess
{
    [Table("user_roles")]
    [Index(nameof(UserId), nameof(RoleId), Name = "user_roles_user_id_role_id_key", IsUnique = true)]
    public partial class UserRole
    {
        [Key]
        [Column("user_role_id")]
        public int UserRoleId { get; set; }
        [Required]
        [Column("user_id", TypeName = "character varying")]
        public string UserId { get; set; }
        [Required]
        [Column("role_id", TypeName = "character varying")]
        public string RoleId { get; set; }
        [Column("created_at", TypeName = "timestamp with time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at", TypeName = "timestamp with time zone")]
        public DateTime UpdatedAt { get; set; }

        [ForeignKey(nameof(RoleId))]
        [InverseProperty("UserRoles")]
        public virtual Role Role { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("UserRoles")]
        public virtual User User { get; set; }
    }
}
