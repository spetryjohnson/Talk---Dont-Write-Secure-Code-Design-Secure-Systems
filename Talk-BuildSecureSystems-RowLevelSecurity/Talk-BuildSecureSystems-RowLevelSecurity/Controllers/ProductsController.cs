using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talk_BuildSecureSystems_RowLevelSecurity.Framework.Authentication;
using Talk_BuildSecureSystems_RowLevelSecurity.Models;

namespace Talk_BuildSecureSystems_RowLevelSecurity.Controllers {

	public class ProductsController : SecuredController {

		[RequiredPermission(PermissionEnum.ManageProducts)]
		public ActionResult Manage() {
			return View();
		}
	}
}