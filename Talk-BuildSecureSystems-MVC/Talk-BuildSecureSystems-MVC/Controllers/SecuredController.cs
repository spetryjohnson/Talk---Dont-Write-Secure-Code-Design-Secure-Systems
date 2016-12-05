using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Talk_BuildSecureSystems_MVC.Framework.Authentication;
using Talk_BuildSecureSystems_MVC.Framework.Extensions;
using Talk_BuildSecureSystems_MVC.Models;

namespace Talk_BuildSecureSystems_MVC.Controllers {
	public class SecuredController : Controller {

		public ApplicationUserManager UserManager {
			get {
				return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
			private set {
				_userManager = value;
			}
		}
		private ApplicationUserManager _userManager;


		public ApplicationUser CurrentUser {
			get {
				if (this.m_currentUser == null) {
					if (User.Identity.IsAuthenticated) {
						this.m_currentUser = UserManager.FindById(User.Identity.GetUserId());
					}
				}
				return this.m_currentUser;
			}
			set { this.m_currentUser = value; }
		}
		private ApplicationUser m_currentUser;


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