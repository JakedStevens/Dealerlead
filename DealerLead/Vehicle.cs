using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerLead
{
	public class Vehicle
	{
		[Key]
		[Column("VehicleId")]
		public int Id { get; set; }

		[Display(Name = "Model")]
		public int ModelId { get; set; }

		public decimal MSRP { get; set; }

		[Display(Name = "Stock Number")]
		public string StockNumber { get; set; }

		public string Color { get; set; }

		[Display(Name = "Dealership")]
		public int DealershipId { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime CreateDate { get; set; }

		public DateTime? ModifyDate { get; set; }

		[Display(Name = "Date of Sale")]
		public DateTime? SellDate { get; set; }

		public Dealership Dealership { get; set; }

		public SupportedModel Model { get; set; }

		public List<Lead> Leads { get; set; }
	}
}
