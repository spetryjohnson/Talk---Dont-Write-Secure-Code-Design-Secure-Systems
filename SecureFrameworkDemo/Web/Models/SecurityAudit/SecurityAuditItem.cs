using System;

namespace SecureFrameworkDemo.Models {

	public class ControllerAuditItem {

		public Type Type { get; set; }
		public bool UsesBaseController { get; set; }
		public bool UsesSecureController { get; set; }

	}
}