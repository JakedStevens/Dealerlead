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

		public int ModelId { get; set; }

		public decimal MSRP { get; set; }

		public string StockNumber { get; set; }

		public string Color { get; set; }

		public string DealershipId { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime CreateDate { get; set; }

		public DateTime? ModifyDate { get; set; }

		public DateTime? SellDate { get; set; }

		public SupportedModel SupportedModel {  get; set; }
	}
}
