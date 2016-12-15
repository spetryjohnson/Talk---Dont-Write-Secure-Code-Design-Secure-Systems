using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SecureFrameworkDemo.Framework.WebPageAuthentication;
using SecureFrameworkDemo.Models;

namespace SecureFrameworkDemo.Controllers {

	public class ProductsController : SecuredController {

		[RequiredPermission(PermissionEnum.ManageProducts)]
		public ActionResult Manage() {
			return View();
		}
	}
}