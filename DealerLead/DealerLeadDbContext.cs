using Microsoft.EntityFrameworkCore;
using System;

namespace DealerLead
{
	public class DealerLeadDbContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder options)
			=> options.UseSqlServer("Server=.;Database=DealerLead;Trusted_Connection=True");

		public DbSet<SupportedState> SupportedState { get; set; }
		public DbSet<SupportedMake> SupportedMake { get; set; }
		public DbSet<SupportedModel> SupportedModel { get; set; }
		public DbSet<DealerLeadUser> DealerLeadUser { get; set; }
		public DbSet<Dealership> Dealership { get; set; }
		public DbSet<Vehicle> Vehicle { get; set; }
        public DbSet<Lead> Lead { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SupportedModel>()
                .HasOne(x => x.Make)
                .WithMany(i => i.Models)
                .HasForeignKey(j => j.MakeId);
            modelBuilder.Entity<Vehicle>()
                .HasOne(x => x.Dealership)
                .WithMany(i => i.Vehicles)
                .HasForeignKey(j => j.DealershipId);
            modelBuilder.Entity<Vehicle>()
                .HasOne(x => x.Model)
                .WithMany(i => i.Vehicles)
                .HasForeignKey(j => j.ModelId);
            modelBuilder.Entity<Lead>()
                .HasMany(x => x.Vehicles)
                .WithMany(i => i.Leads);
        }
    }
}
