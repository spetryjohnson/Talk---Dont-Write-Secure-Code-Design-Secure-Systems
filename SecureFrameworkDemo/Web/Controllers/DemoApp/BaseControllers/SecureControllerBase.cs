using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SecureFrameworkDemo.Framework;
using SecureFrameworkDemo.Framework.WebPageAuthentication;
using System.Configuration;

namespace SecureFrameworkDemo.Controllers {

	public class SecureControllerBase : BaseController {

		protected override void OnActionExecuting(ActionExecutingContext context) {
			var pageRequiresLogin = PathRequiresAuthentication(
				context.RequestContext.HttpContext.Request.Path
			);

			var userIsLoggedIn = User.Identity.IsAuthenticated;
			var userIsNotLoggedIn = !userIsLoggedIn;

			// enforce authentication check
			if (pageRequiresLogin && userIsNotLoggedIn) {
				context.Result = new HttpUnauthorizedResult();
				return;
			}

			// enforce authorization rules
			var requiredPermAttr = context.ActionDescriptor.GetAttribute<RequiredPermissionAttribute>();
			var pageRequiresSpecificPermission = (requiredPermAttr != null);

			if (pageRequiresLogin && userIsLoggedIn && pageRequiresSpecificPermission) {
				var hasRequiredPerm = CurrentUser.Permissions.Any(
					p => p.Id == (int)requiredPermAttr.Permission
				);

				if (!hasRequiredPerm) {
					context.Result = new HttpUnauthorizedResult();
					return;
				}
			}

			base.OnActionExecuting(context);
		}

		/// <summary>
		/// Returns TRUE if the specified URL requires the user be authenticated, or FALSE if
		/// the URL is publicly accessible.
		/// </summary>
		/// <param name="requestedPath">
		///		The absolute path of the request. This should NOT include "http://" portion. This MAY include
		///		querystring data, which is ignored.
		///	</param>
		protected static bool PathRequiresAuthentication(string requestedPath) {
			// strip the querystring
			var path = requestedPath.Split("?")[0];

			// check the list of "publicly available URLs" in web config
			var publicPaths = (CustomAuthenticationSection)ConfigurationManager.GetSection("customAuthentication");

			if (publicPaths != null && publicPaths.AllowsAnonymousAccess(path)) {
				return false;
			}

			// if not explicitly allowed as public, then it's private
			return true;
		}
	}
}