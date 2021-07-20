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
	}
}
