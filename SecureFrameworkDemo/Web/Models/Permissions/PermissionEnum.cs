using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SecureFrameworkDemo.Models {

	public enum PermissionEnum {
		
		/// <summary>
		/// Basic access to the site
		/// </summary>
		BasicPrivileges = 1,

		/// <summary>
		/// Grants access to Orders you don't own
		/// </summary>
		ManageOrders = 100,

		/// <summary>
		/// Allows you to see a user's SSN, unmasked
		/// </summary>
		ViewSSN = 200,

		API_ViewOrders = 300
	}
}
