using System;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace SecureFrameworkDemo.Framework.AntiCSRF {

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class ValidateAntiForgeryTokenAjaxAttribute : FilterAttribute, IAuthorizationFilter {

		public void OnAuthorization(AuthorizationContext context) {

			// if this is added to a request that doesn't use ajax, just use the default
			var isAjax = context.HttpContext.Request.IsAjaxRequest();

			if (!isAjax) {
				AntiForgery.Validate();
			}
			else {
				ValidateAntiForgeryHeader(context);
			}
		}

		private void ValidateAntiForgeryHeader(AuthorizationContext context) {
			var request = context.HttpContext.Request;

			var headerToken = request.Headers["__RequestVerificationToken"];
			var cookieToken = string.Empty;
			var formToken = string.Empty;

			try {
				var tokens = headerToken.Split(',');
				cookieToken = tokens[0].Trim();
				formToken = tokens[1].Trim();
			}
			catch (Exception) {
				// ignore it - something wasn't formatted correctly, so let it fail
			}

			AntiForgery.Validate(cookieToken, formToken); 
		}
	}
}