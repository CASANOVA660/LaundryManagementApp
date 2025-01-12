// LaundryManagement.ConsoleApp/Data/LaundryDbContext.cs
using Microsoft.EntityFrameworkCore;
using LaundryManagement.ConsoleApp.Models;

namespace LaundryManagement.ConsoleApp.Data
{
    public class LaundryDbContext : DbContext
    {
        public DbSet<Laundry> Laundries { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<Cycle> Cycles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=LaundryManagementDb;Trusted_Connection=True;");
        }
    }
}