using System;
using System.Web.Mvc;
using System.Configuration;
using System.Web.Helpers;
using SecureFrameworkDemo.Framework;
using SecureFrameworkDemo.Framework.WebPageAuthentication;

namespace SecureFrameworkDemo.Controllers {

	public class SecureControllerBase : BaseController {

		protected override void OnActionExecuting(ActionExecutingContext ctx) {
			var pageRequiresLogin = PathRequiresAuthentication(
				ctx.RequestContext.HttpContext.Request.Path
			);

			var userIsNotLoggedIn = !User.Identity.IsAuthenticated;

			// AUTHENTICATION
			if (pageRequiresLogin && userIsNotLoggedIn) {
				ctx.Result = new HttpUnauthorizedResult();
				return;
			}

			// AUTHORIZATION (handled by the [RequiredPermission] attribute)

			// ANTI-CSRF: basically like adding [ValidateAntiForgeryToken] globally to all actions
			if (ctx.HttpContext.Request.HttpMethod == "POST") {
				ValidateAntiForgeryToken(ctx);
			}

			base.OnActionExecuting(ctx);
		}

		/// <summary>
		/// Returns TRUE if the specified URL requires the user be authenticated, or FALSE if
		/// the URL is publicly accessible.
		/// </summary>
		/// <param name="requestedPath">
		///		The absolute path of the request. This should NOT include "http://" portion. This MAY include
		///		querystring data, which is ignored.
		///	</param>
		public static bool PathRequiresAuthentication(string requestedPath) {
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

		/// <summary>
		/// All forms using the secure controller base are expected to have an anti-CSRF token, which is added
		/// dynamically to all forms using some JS.
		/// </summary>
		private void ValidateAntiForgeryToken(ActionExecutingContext context) {
			var formToken = context.HttpContext.Request.Form["__RequestVerificationToken"];
			var cookieToken = context.HttpContext.Request.Cookies["__RequestVerificationToken"].Value;
			AntiForgery.Validate(cookieToken, formToken);
		}
	}
}