using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkManagermentWeb.Domain.Entities;

namespace WorkManagermentWeb.Infrastructure.Data
{
    /// <summary>
    /// AppDBContext
    /// </summary>
    public class AppDBContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// AppDBContext
        /// </summary>
        /// <param name="options"></param>
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        /// <summary>
        /// OnModelCreating
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
            modelBuilder.Entity<ApplicationUser>().ToTable("Users").Property(p => p.Id).HasColumnName("Id");
        }

        /// <summary>
        /// Boards
        /// </summary>
        public DbSet<Board> Boards { get; set; }

        /// <summary>
        /// BoardUsers
        /// </summary>
        public DbSet<BoardUser> BoardUsers { get; set; }

        /// <summary>
        /// Comments
        /// </summary>
        public DbSet<Comment> Comments { get; set; }

        /// <summary>
        /// Notifications
        /// </summary>
        public DbSet<Notification> Notifications { get; set; }

        /// <summary>
        /// NotificationLanguages
        /// </summary>
        public DbSet<NotificationLanguage> NotificationLanguages { get; set; }

        /// <summary>
        /// WorkItems
        /// </summary>
        public DbSet<WorkItem> WorkItems { get; set; }

        /// <summary>
        /// WorkSpaces
        /// </summary>
        public DbSet<WorkSpace> WorkSpaces { get; set; }

        /// <summary>
        /// WorkSpaceUsers
        /// </summary>
        public DbSet<WorkSpaceUser> WorkSpaceUsers { get; set; }

        /// <summary>
        /// CalendarEvents
        /// </summary>
        public DbSet<CalendarEvent> CalendarEvents { get; set; }
    }
}
