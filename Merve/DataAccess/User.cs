using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Merve.Enums;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Merve.DataAccess
{
    [Table("users")]
    [Index(nameof(Email), Name = "users_email_key", IsUnique = true)]
    [Index(nameof(Gsm), Name = "users_gsm_key", IsUnique = true)]
    [Index(nameof(SecondaryEmail), Name = "users_secondary_email_key", IsUnique = true)]
    public partial class User
    {
        public User()
        {
            UserRoles = new HashSet<UserRole>();
        }

        [Key]
        [Column("user_id", TypeName = "character varying")]
        public string UserId { get; set; }
        [Column("email", TypeName = "character varying")]
        public string Email { get; set; }
        [Required]
        [Column("password", TypeName = "character varying")]
        public string Password { get; set; }
        [Column("secondary_email", TypeName = "character varying")]
        public string SecondaryEmail { get; set; }
        [Column("gsm", TypeName = "character varying")]
        public string Gsm { get; set; }
        [Required]
        [Column("firstname", TypeName = "character varying")]
        public string Firstname { get; set; }
        [Required]
        [Column("lastname", TypeName = "character varying")]
        public string Lastname { get; set; }
        [Required]
        [Column("locale", TypeName = "character varying")]
        public string Locale { get; set; }
        [Required]
        [Column("timezone", TypeName = "character varying")]
        public string Timezone { get; set; }
        [Column("created_at", TypeName = "timestamp with time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at", TypeName = "timestamp with time zone")]
        public DateTime UpdatedAt { get; set; }

        //public USER_STATUS status { get; set; }

        //public string status { get; set; }

        [InverseProperty(nameof(UserRole.User))]
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
