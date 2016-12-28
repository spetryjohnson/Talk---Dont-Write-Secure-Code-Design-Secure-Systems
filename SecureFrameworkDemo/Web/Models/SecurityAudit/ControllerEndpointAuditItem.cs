using System;

namespace SecureFrameworkDemo.Models {

	public class ControllerEndpointAuditItem {

		public string Controller { get; set; }
		public string Action { get; set; }
		public bool RequiresAuthentication { get; set; }
		public PermissionEnum? RequiresPermission { get; set; }
	}
}