using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerLead
{
	public class Lead
	{
		[Key]
		[Column("LeadId")]
		public int Id { get; set; }

		[Display(Name = "First Name")]
		public string FirstName { get; set; }

		[Display(Name = "Last Name")]
		public string LastName { get; set; }

		[Display(Name = "Email")]
		public string EmailAddress { get; set; }

		[Display(Name = "Phone")]
		public string PhoneNumber { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime CreateDate { get; set; }

		public DateTime? ModifyDate { get; set; }

		public List<Vehicle> Vehicles { get; set; }
		//public Dealership Dealership { get; set; }
		//public SupportedModel Model { get; set; }
	}
}
