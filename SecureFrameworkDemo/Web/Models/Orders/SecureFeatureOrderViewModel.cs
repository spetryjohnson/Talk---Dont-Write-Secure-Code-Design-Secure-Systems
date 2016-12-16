using System;
using System.Diagnostics.Contracts;

namespace SecureFrameworkDemo.Models {

	/// <summary>
	/// View model for an Order.
	/// 
	/// This is the "secure feature" version which implements the SSN masking.
	/// </summary>
	public class SecureFeatureOrderViewModel {
		public int Id { get; set; }
		public DateTime PlacedOn { get; set; }
		public string UserName { get; set; }
		public string SSN { get; set; }

		public SecureFeatureOrderViewModel(Order o, ApplicationUser currentUser) {
			Contract.Requires(o != null);

			Id = o.Id;
			PlacedOn = o.PlacedOn;
			UserName = o.ApplicationUser.UserName;

			// Need a special permission to see the unmasked value
			SSN = currentUser.HasPermission(PermissionEnum.ViewSSN)
				? o.ApplicationUser.SSN
				: "***-**-****";
		}
	}
}