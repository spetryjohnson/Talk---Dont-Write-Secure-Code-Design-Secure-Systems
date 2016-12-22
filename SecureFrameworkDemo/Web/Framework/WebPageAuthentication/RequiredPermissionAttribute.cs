using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SecureFrameworkDemo.Models;
using SecureFrameworkDemo.Controllers;

namespace SecureFrameworkDemo.Framework.WebPageAuthentication {

	/// <summary>
	/// MVC Action Filter that requires the current user to have the specified permission.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
	public class RequiredPermissionAttribute : ActionFilterAttribute {

		public PermissionEnum Permission { get; private set; }

		public RequiredPermissionAttribute(PermissionEnum permission) {
			Permission = permission;
		}

		public override void OnActionExecuting(ActionExecutingContext context) {
			var currentUser = ((SecureControllerBase)(context.Controller)).CurrentUser;

			var hasRequiredPerm = currentUser.Permissions.Any(
				p => p.Id == (int)this.Permission
			);

			if (!hasRequiredPerm) {
				context.Result = new HttpUnauthorizedResult();
				return;
			}
		}
	}
}