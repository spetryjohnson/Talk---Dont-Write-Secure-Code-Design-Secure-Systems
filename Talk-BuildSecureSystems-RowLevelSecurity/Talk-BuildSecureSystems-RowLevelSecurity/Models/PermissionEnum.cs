using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Talk_BuildSecureSystems_RowLevelSecurity.Models {

	public enum PermissionEnum {
		BasicPrivileges = 1,
		ManageProducts = 100,
		ViewOrdersForOthers = 200
	}
}
