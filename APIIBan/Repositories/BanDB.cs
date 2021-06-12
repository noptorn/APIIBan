using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIIBan.Model;
using Microsoft.EntityFrameworkCore;

namespace APIIBan.Repositories
{
    public class BanDB : DbContext
    {
        public BanDB() { }

        public BanDB(DbContextOptions<BanDB> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //       optionsBuilder.UseLoggerFactory(loggerFactory); 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Account>()
                .ToTable("ACCOUNTS")
                .HasKey(k => k.AccountID);

            modelBuilder
                .Entity<Transaction>()
                .ToTable("TRANSACTIONS")
                .HasKey(k => new { k.AccountID, k.TransID}) ;

        }
    }
}
