using System;
using System.Web.Mvc;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;

namespace SecureFrameworkDemo.Controllers {

	/// <summary>
	/// Convenience controller that makes it easy to switch between demo accounts in the app.
	/// </summary>
	public class AutoLoginController : BaseController {

		protected ApplicationSignInManager SignInManager {
			get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
			private set { _signInManager = value; }
		}
		private ApplicationSignInManager _signInManager;

		public async Task<ActionResult> AutoLogin(string user, string returnUrl) {
			var result = await SignInManager.PasswordSignInAsync(user, "Passw0rd", isPersistent: false, shouldLockout: false);

			switch (result) {
				case SignInStatus.Success:
					return Redirect(returnUrl ?? "/");
				default:
					throw new ApplicationException("Auto login had result: " + result.ToString());
			}
		}
	}
}