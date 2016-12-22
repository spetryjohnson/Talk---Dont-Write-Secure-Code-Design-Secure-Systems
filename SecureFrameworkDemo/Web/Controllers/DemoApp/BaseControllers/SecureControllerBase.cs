using System;
using System.Web.Mvc;
using System.Configuration;
using System.Web.Helpers;
using SecureFrameworkDemo.Framework;
using SecureFrameworkDemo.Framework.WebPageAuthentication;

namespace SecureFrameworkDemo.Controllers {

	public class SecureControllerBase : BaseController {

		protected override void OnActionExecuting(ActionExecutingContext context) {
			var pageRequiresLogin = PathRequiresAuthentication(
				context.RequestContext.HttpContext.Request.Path
			);

			var userIsLoggedIn = User.Identity.IsAuthenticated;
			var userIsNotLoggedIn = !userIsLoggedIn;

			// AUTHENTICATION: make sure user is authenticated if accessing private resource
			if (pageRequiresLogin && userIsNotLoggedIn) {
				context.Result = new HttpUnauthorizedResult();
				return;
			}

			// AUTHORIZATION: handled by the [RequiredPermission] attribute

			// ANTI-CSRF: basically like adding [ValidateAntiForgeryToken] globally to all actions
			if (context.HttpContext.Request.HttpMethod == "POST") {
				ValidateAntiForgeryToken(context);
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