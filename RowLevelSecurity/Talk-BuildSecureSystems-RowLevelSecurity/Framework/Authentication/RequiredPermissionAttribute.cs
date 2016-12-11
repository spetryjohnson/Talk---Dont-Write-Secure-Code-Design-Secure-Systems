using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Talk_BuildSecureSystems_RowLevelSecurity.Models;

namespace Talk_BuildSecureSystems_RowLevelSecurity.Framework.Authentication {

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
	public class RequiredPermissionAttribute : Attribute {

		public PermissionEnum Permission { get; private set; }

		public RequiredPermissionAttribute(PermissionEnum permission) {
			Permission = permission;
		}
	}
}