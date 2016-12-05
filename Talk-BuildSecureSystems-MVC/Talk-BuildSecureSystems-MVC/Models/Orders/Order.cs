using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Talk_BuildSecureSystems_MVC.Models.Orders {

	public class Order {

		[Key]
		public int Id { get; set; }
		public virtual ApplicationUser ApplicationUser { get; set; }
		public DateTime PlacedOn { get; set; }

		public Order() {
			PlacedOn = DateTime.Now;
		}

	}
}