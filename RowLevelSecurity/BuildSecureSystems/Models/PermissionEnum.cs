using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BuildSecureSystems.Models {

	public enum PermissionEnum {
		BasicPrivileges = 1,
		ManageProducts = 100,
		ViewOrdersForOthers = 200,
		API_ViewOrders = 300
	}
}
