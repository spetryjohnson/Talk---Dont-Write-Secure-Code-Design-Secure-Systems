using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talk_BuildSecureSystems_MVC.Framework.Authentication;
using Talk_BuildSecureSystems_MVC.Models;

namespace Talk_BuildSecureSystems_MVC.Controllers {

	public class ProductsController : SecuredController {

		[RequiredPermission(PermissionEnum.ManageProducts)]
		public ActionResult Manage() {
			return View();
		}
	}
}