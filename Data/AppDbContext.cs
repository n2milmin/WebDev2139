﻿using Microsoft.EntityFrameworkCore;
using Lab2.Models;

namespace Lab2.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}

		public DbSet<Project> Projects { get; set; }
		public DbSet<ProjectTask> ProjectTasks { get; set; }
	}
}