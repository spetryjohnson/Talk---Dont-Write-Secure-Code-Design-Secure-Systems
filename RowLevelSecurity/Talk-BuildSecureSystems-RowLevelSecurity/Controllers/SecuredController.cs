using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Talk_BuildSecureSystems_RowLevelSecurity.Framework.Authentication;
using Talk_BuildSecureSystems_RowLevelSecurity.Framework.Extensions;

namespace Talk_BuildSecureSystems_RowLevelSecurity.Controllers {
	public class SecuredController : BaseController {

		protected override void OnActionExecuting(ActionExecutingContext context) {

			var permAttribute = context.ActionDescriptor.GetAttribute<RequiredPermissionAttribute>();

			if (permAttribute == null) {
				return;
			}

			var isLoggedIn = (CurrentUser != null);
			var requiredPerm = permAttribute.Permission;
			var hasRequiredPerm = isLoggedIn && CurrentUser.Permissions.Any(p => p.Id == (int)requiredPerm);

			if (!hasRequiredPerm) {
				context.Result = new HttpUnauthorizedResult();
				return;
			}
		}
	}
}