using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace SecureFrameworkDemo.Models {

	public class ViewOrderModel {
		public int Id { get; set; }
		public DateTime PlacedOn { get; set; }
		public string CustomerUserName { get; set; }

		public ViewOrderModel(Order o) {
			Contract.Requires(o != null);

			Id = o.Id;
			PlacedOn = o.PlacedOn;
			CustomerUserName = o.ApplicationUser.UserName;
		}
	}
}