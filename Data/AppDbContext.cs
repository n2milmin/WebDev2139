using Microsoft.EntityFrameworkCore;
using Lab2.Areas.ProjectManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Lab2.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<ProjectComment> ProjectComments { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Project>().HasData(
                new Project
                {
                    ProjectId = 1,
                    Name = "COMP2139 Assignment 1",
                    Description = "First COMP2139 Assignment",
                    StartDate = new DateTime(2024, 1, 8),
                    EndDate = new DateTime(2024, 2, 24),
                    Status = "In Progress"
                },
                new Project
                {
                    ProjectId = 2,
                    Name = "COMP2139 Assignment 2",
                    Description = "Second COMP2139 Assignment",
                    StartDate = new DateTime(2024, 3, 15),
                    EndDate = new DateTime(2024, 4, 16),
                    Status = "In Progress"
                });

            builder.Entity<ProjectTask>().HasData(
                new ProjectTask
                {
                    ProjectTaskId = 1,
                    Title = "Database Design",
                    Description = "Database Design for Assignment 1",
                    ProjectId = 1
                },
                new ProjectTask
                {
                    ProjectTaskId = 2,
                    Title = "Authentication",
                    Description = "Authentication Using Identity Core",
                    ProjectId = 2
                });

            builder.HasDefaultSchema("Identity");

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "User");
            });

            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable(name: "UserRoles");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable(name: "UserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable(name: "UserLogins");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable(name: "RoleClaims");
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable(name: "UserToken");
            });
        }
    }
}