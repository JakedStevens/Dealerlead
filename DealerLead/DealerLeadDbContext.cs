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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SupportedModel>()
                .HasOne(p => p.Make)
                .WithMany(b => b.Models)
                .HasForeignKey(p => p.MakeId);
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.Dealership)
                .WithMany(d => d.Vehicles)
                .HasForeignKey(v => v.DealershipId);
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.Model)
                .WithMany(m => m.Vehicles)
                .HasForeignKey(v => v.ModelId);
        }
    }
}
