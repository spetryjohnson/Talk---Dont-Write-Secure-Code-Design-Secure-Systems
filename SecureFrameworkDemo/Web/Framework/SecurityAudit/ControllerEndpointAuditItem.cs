using SecureFrameworkDemo.Models;
using System;

namespace SecureFrameworkDemo.Framework.SecurityAudit {

	public class ControllerEndpointAuditItem {

		public string Controller { get; set; }
		public string Action { get; set; }
		public bool RequiresAuthentication { get; set; }
		public PermissionEnum? RequiresPermission { get; set; }

		public string RelativePath {
			get { return $"{Controller}/{Action}"; }
		}
	}
}