using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TulipInfo.Net.EFCore.Tests
{
    public class TestDbContext : DbContext
    {
        public DbSet<EntityOne> EntityOnes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("data source=xxx;initial catalog=db;User Id=sa;Password=xxx;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var map1 = modelBuilder.Entity<EntityOne>();
            map1.ToTable("EntityOnes");
            map1.Property(e => e.UserName).HasColumnType("varchar").HasMaxLength(100);
            map1.Property(e => e.DisplayName).HasColumnName("FullName").HasColumnType("nvarchar").HasMaxLength(200);
        }
    }
}
