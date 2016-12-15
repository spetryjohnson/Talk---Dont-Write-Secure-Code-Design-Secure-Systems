using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BuildSecureSystems.Models;

namespace BuildSecureSystems.Framework.WebPageAuthentication {

	/// <summary>
	/// MVC Action Filter that requires the current user to have the specified permission.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
	public class RequiredPermissionAttribute : Attribute {

		public PermissionEnum Permission { get; private set; }

		public RequiredPermissionAttribute(PermissionEnum permission) {
			Permission = permission;
		}
	}
}