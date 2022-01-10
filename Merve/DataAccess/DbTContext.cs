using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;


namespace Merve.DataAccess
{
    public partial class DbTContext : DbContext
    {
        public DbTContext()
        {
        }

        public DbTContext(DbContextOptions<DbContext> options): base(options)
        {
        }

        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Server=localhost;Database=merve;Username=postgres;Password=88726789t");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum(null, "user_status", new[] { "ACTIVE", "INACTIVE" });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");
            });

            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(d => d.PermissionId)
                    .HasConstraintName("role_permissions_permission_id_fkey");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("role_permissions_role_id_fkey");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");

                entity.Property(e => e.Locale).HasDefaultValueSql("'tr'::character varying");

                entity.Property(e => e.Timezone).HasDefaultValueSql("'Asia/Istanbul'::character varying");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("user_roles_role_id_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("user_roles_user_id_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
