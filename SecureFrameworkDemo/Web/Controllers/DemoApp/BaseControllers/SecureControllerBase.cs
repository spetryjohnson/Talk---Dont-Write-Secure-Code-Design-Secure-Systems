using System;
using System.Web.Mvc;
using System.Configuration;
using System.Web.Helpers;
using SecureFrameworkDemo.Framework;
using SecureFrameworkDemo.Framework.WebPageAuthentication;

namespace SecureFrameworkDemo.Controllers {

	public class SecureControllerBase : BaseController {

		protected override void OnActionExecuting(ActionExecutingContext ctx) {
			// AUTHENTICATION is handled by PrivateByDefaultAuthModule		

			// AUTHORIZATION is handled by the [RequiredPermission] attribute

			// ANTI-CSRF: basically like adding [ValidateAntiForgeryToken] globally to all actions
			if (ctx.HttpContext.Request.HttpMethod == "POST") {
				ValidateAntiForgeryToken(ctx);
			}

			base.OnActionExecuting(ctx);
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