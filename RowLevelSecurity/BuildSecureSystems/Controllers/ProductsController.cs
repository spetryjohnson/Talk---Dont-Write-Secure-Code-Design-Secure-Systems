using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BuildSecureSystems.Framework.WebPageAuthentication;
using BuildSecureSystems.Models;

namespace BuildSecureSystems.Controllers {

	public class ProductsController : SecuredController {

		[RequiredPermission(PermissionEnum.ManageProducts)]
		public ActionResult Manage() {
			return View();
		}
	}
}