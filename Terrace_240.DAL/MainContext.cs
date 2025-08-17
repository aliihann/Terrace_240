using Microsoft.EntityFrameworkCore;
using System.Data;
using Terrace_240.DAL.Model;

namespace Terrace_240.DAL
{
    public class MainContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<AdminPassword> AdminPasswords { get; set; }

        public MainContext(DbContextOptions<MainContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<AdminPassword>().HasData(new List<AdminPassword>()
            //{
            //    new() {Id = 1, PasswordHash = "admin123",CreatedAt = DateTime.Now}
            //});

            //modelBuilder.Entity<Movie>().HasData(new List<Movie>()
            //{
            //   new() {Id = 1, Title="Интерстеллар", Year=2014, Genre="sci-fi", Rating=8.6, PosterUrl="", Description="Фантастический фильм.", ScheduleTimes="20:00,22:30", Price=300 },
            //   new() {Id = 2, Title="Побег из Шоушенка", Year=1994, Genre="drama", Rating=9.3, PosterUrl="", Description="Драма о надежде.", ScheduleTimes="19:30,22:00", Price=300 }
            //});
        }
    }
}
