using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BuildSecureSystems.Models;

namespace BuildSecureSystems.Framework.Authentication {

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
	public class RequiredPermissionAttribute : Attribute {

		public PermissionEnum Permission { get; private set; }

		public RequiredPermissionAttribute(PermissionEnum permission) {
			Permission = permission;
		}
	}
}