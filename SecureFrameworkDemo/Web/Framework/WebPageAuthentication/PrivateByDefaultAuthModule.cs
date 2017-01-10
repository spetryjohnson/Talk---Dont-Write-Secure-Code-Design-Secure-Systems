using System;
using System.Configuration;
using System.Security.Permissions;
using System.Web;

namespace SecureFrameworkDemo.Framework.WebPageAuthentication {

	/// <summary>
	/// Enforces the authentication rules specified in the "customAuthentication" section of web.config.
	/// 
	/// Allows us to take a secure-by-default approach to ALL web requests, even for static resources,
	/// and to control public access from a centralized place.
	/// </summary>
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	class PrivateByDefaultAuthModule : IHttpModule {

		public virtual void Dispose() {
		}

		/// <summary>
		/// Initializes the module, sets up event handlers, etc.
		/// </summary>
		public virtual void Init(HttpApplication p_httpApp) {
			p_httpApp.AcquireRequestState += OnAcquireRequestState;
		}

		/// <summary>
		/// If the user is requesting a protected resource via a GET, and is not logged in, they are redirected
		/// to the login page. Otherwise, nothing happens.
		/// </summary>
		/// <param name="sender">The active HttpApplication.</param>
		/// <param name="e">Not used.</param>
		private void OnAcquireRequestState(object sender, EventArgs e) {

			var application = (HttpApplication)sender;
			var context = (HttpContext)(application.Context);
			var isAlreadyLoggedIn = context?.User?.Identity?.IsAuthenticated ?? false;
			var urlAllowsPublicAcccess = !PathRequiresAuthentication(context.Request.Path);

			if (urlAllowsPublicAcccess) { return; }
			if (isAlreadyLoggedIn) { return; }

			context.Response.Redirect(
				"/account/login?returnUrl=" + HttpUtility.UrlEncode(context.Request.Path)
			);
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
	}
}
