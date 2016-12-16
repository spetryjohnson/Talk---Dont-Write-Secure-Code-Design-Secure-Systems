using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace SecureFrameworkDemo.Models {

	/// <summary>
	/// View model for an Order.
	/// 
	/// This is the insecure version - it doesn't do any security restrictions.
	/// </summary>
	public class InsecureOrderViewModel {
		public int Id { get; set; }
		public DateTime PlacedOn { get; set; }
		public string UserName { get; set; }
		public string SSN { get; set; }

		public InsecureOrderViewModel(Order o) {
			Contract.Requires(o != null);

			Id = o.Id;
			PlacedOn = o.PlacedOn;
			UserName = o.ApplicationUser.UserName;
			SSN = o.ApplicationUser.SSN;
		}
	}
}