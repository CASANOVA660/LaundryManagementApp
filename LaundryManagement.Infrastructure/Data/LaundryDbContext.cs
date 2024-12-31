using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LaundryManagement.Infrastructure.Models;

namespace LaundryManagement.Infrastructure.Data
{
    public class LaundryDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Laundry> Laundries { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<Cycle> Cycles { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public LaundryDbContext(DbContextOptions<LaundryDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define relationships
            modelBuilder.Entity<Laundry>()
                .HasOne(l => l.Proprietaire)
                .WithMany()
                .HasForeignKey(l => l.IdProprietaire);

            modelBuilder.Entity<Machine>()
                .HasOne(m => m.Laundry)
                .WithMany(l => l.Machines)
                .HasForeignKey(m => m.IdLaundry);

            modelBuilder.Entity<Cycle>()
                .HasOne(c => c.Machine)
                .WithMany(m => m.Cycles)
                .HasForeignKey(c => c.IdMachine);
            modelBuilder.Entity<Cycle>()
                .Property(c => c.Cost)
                .HasPrecision(18, 2); // 18 total digits, 2 digits after the decimal point
            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2); // 18 total digits, 2 digits after the decimal point
        }
    }
}


