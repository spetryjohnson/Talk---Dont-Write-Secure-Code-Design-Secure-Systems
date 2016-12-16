using System;
using System.Diagnostics.Contracts;
using SecureFrameworkDemo.Framework.PropertyAccessAuthorization;

namespace SecureFrameworkDemo.Models {

	/// <summary>
	/// View model for an Order.
	/// 
	/// This is the "secure framework" version which implements the SSN masking via an PostSharp aspect.
	/// </summary>
	public class SecureFrameworkOrderViewModel {
		public int Id { get; set; }
		public DateTime PlacedOn { get; set; }
		public string UserName { get; set; }

		/// <summary>
		/// PostSharp aspect makes it easy to implement access control
		/// </summary>
		[MaskedValue(PermissionEnum.ViewSSN)]
		public string SSN { get; set; }

		public SecureFrameworkOrderViewModel(Order o) {
			Contract.Requires(o != null);

			Id = o.Id;
			PlacedOn = o.PlacedOn;
			UserName = o.ApplicationUser.UserName;
			SSN = o.ApplicationUser.SSN;
		}
	}
}