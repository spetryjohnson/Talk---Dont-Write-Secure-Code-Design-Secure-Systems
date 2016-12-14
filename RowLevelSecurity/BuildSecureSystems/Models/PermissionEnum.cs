using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BuildSecureSystems.Models {

	public enum PermissionEnum {
		BasicPrivileges = 1,
		ManageProducts = 100,
		ManageUsers = 150,
		ViewOrdersForOthers = 200,
		ViewCreditCard = 250,
		ViewSSN = 251,
		API_ViewOrders = 300
	}
}
