using QExpress.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QExpress.Data
{
    public class QExpressDbContext : ApiAuthorizationDbContext<Felhasznalo>
    {
        public DbSet<Felhasznalo> Felhasznalo { get; set; }
        public DbSet<Ceg> Ceg { get; set; }
        public DbSet<Telephely> Telephely { get; set; }
        public DbSet<Kategoria> Kategoria { get; set; }
        public DbSet<Sorszam> Sorszam { get; set; }
        public DbSet<FelhasznaloTelephely> FelhasznaloTelephely { get; set; }
        public DbSet<UgyfLevelek> UgyfLevelek { get; set; }

        public QExpressDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Felhasznalo>().HasKey(f => f.Id);
            modelBuilder.Entity<Ceg>();
            modelBuilder.Entity<Telephely>();
            modelBuilder.Entity<Kategoria>();
            modelBuilder.Entity<Sorszam>();
            modelBuilder.Entity<UgyfLevelek>();

            modelBuilder.Entity<FelhasznaloTelephely>().HasKey(ft => new { ft.FelhasznaloId, ft.TelephelyId });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=aspnet-QExpress;Integrated Security=True");
            }
        }
    }
}
