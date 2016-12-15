using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SecureFrameworkDemo.Models;
using SecureFrameworkDemo.Framework.WebPageAuthentication;

namespace SecureFrameworkDemo.Controllers {

	public class UsersController : SecuredController {

		protected UserService UserSvc {
			get {
				return _userSvc ?? (_userSvc = new UserService(this.AppDbContext));
			}
			set { _userSvc = value; }
		}
		private UserService _userSvc;

		[RequiredPermission(PermissionEnum.ManageUsers)]
		public ActionResult List() {
			ViewBag.Users = UserSvc.GetAll().ToList();

			return View();
		}

		[RequiredPermission(PermissionEnum.ManageUsers)]
		public ActionResult Detail(string id) {
			ViewBag.Model = UserSvc.GetById(id);

			return View();
		}
	}
}