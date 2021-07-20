using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerLead
{
	public class Dealership
	{
		[Key]
		[Column("DealershipId")]
		public int Id { get; set; }

		[Column("DealershipName")]
		public string Name { get; set; }

		[Column("StreetAddress1")]
		[Display(Name = "Street Address 1")]
		public string StreetAddress1 { get; set; }

		[Column("StreetAddress2")]
		[Display(Name = "Street Address 2")]
		public string StreetAddress2 { get; set; }

		public string City { get; set; }

		public string State { get; set; }

		public string Zipcode { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime CreateDate { get; set; }

		public DateTime? ModifyDate { get; set; }

		public int CreatingUserId { get; set; }
	}
}
