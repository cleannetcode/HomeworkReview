using LessonMonitor.DataAccess.MSSQL.Configurations;
using LessonMonitor.DataAccess.MSSQL.Entities;
using Microsoft.EntityFrameworkCore;

namespace LessonMonitor.DataAccess.MSSQL
{
	public class LessonMonitorDbContext : DbContext
	{
		public LessonMonitorDbContext(DbContextOptions<LessonMonitorDbContext> options) : base(options)
		{
		}

		public DbSet<Lesson> Lessons { get; set; }
		public DbSet<Homework> Homeworks { get; set; }
		public DbSet<Member> Members { get; set; }
		public DbSet<Question> Questions { get; set; }
		public DbSet<GitHubAccount> GithubAccounts { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new HomeworkConfiguration());
			modelBuilder.ApplyConfiguration(new LessonConfiguration());
			modelBuilder.ApplyConfiguration(new GithubAccountConfiguration());
			modelBuilder.ApplyConfiguration(new MemberConfiguration());
			modelBuilder.ApplyConfiguration(new QuestionConfiguration());

			base.OnModelCreating(modelBuilder);
		}
	}
}
