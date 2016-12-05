using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talk_BuildSecureSystems_MVC.Framework.Authentication;
using Talk_BuildSecureSystems_MVC.Models;

namespace Talk_BuildSecureSystems_MVC.Controllers {

	public class OrdersController : SecuredController {

		[RequiredPermission(PermissionEnum.BasicPrivileges)]
		public ActionResult View(int id) {
			return View();
		}
	}
}