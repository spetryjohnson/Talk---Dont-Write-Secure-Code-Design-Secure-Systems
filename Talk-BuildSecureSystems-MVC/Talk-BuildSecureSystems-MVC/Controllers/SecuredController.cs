using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talk_BuildSecureSystems_MVC.Framework.Authentication;
using Talk_BuildSecureSystems_MVC.Framework.Extensions;
using Talk_BuildSecureSystems_MVC.Models;

namespace Talk_BuildSecureSystems_MVC.Controllers {
	public class SecuredController : Controller {

		public ApplicationUser CurrentUser {
			get {
				if (this.m_currentUser == null) {
					this.m_currentUser = new ApplicationUser();	// TODO
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

			var requiredPerm = permAttribute.Permission;
			var hasRequiredPerm = CurrentUser.Permissions.Any(p => p.Id == (int)requiredPerm);

			if (!hasRequiredPerm) {
				context.Result = new HttpUnauthorizedResult();
			}
		}
	}
}