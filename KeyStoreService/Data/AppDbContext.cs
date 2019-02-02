using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KeyStoreService.Models;

namespace KeyStoreService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public AppDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("data source=store.db");
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<KeyInfo> KeyInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<KeyInfo>()
                                .HasIndex(k => k.Alias).IsUnique();
            base.OnModelCreating(modelBuilder);
        }

    }
}
