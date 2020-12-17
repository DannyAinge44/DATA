using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Models;

namespace DataAccess
{
    public class AccountingContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public AccountingContext(DbContextOptions<AccountingContext> opt) : base(opt)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasMany(s => s.BookingsFrom)
                .WithOne(s => s.AccountFrom)
                .HasForeignKey(s => s.AccountFromId);
            modelBuilder.Entity<Account>()
                .HasMany(s => s.BookingsTo)
                .WithOne(s => s.AccountTo)
                .HasForeignKey(s => s.AccountToId);
        }
    }
}
