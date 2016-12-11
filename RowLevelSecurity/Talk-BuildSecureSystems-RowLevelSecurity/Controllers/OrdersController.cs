using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talk_BuildSecureSystems_RowLevelSecurity.Framework.Authentication;
using Talk_BuildSecureSystems_RowLevelSecurity.Models;
using Talk_BuildSecureSystems_RowLevelSecurity.Models.Orders;

namespace Talk_BuildSecureSystems_RowLevelSecurity.Controllers {

	public class OrdersController : SecuredController {

		protected OrderService OrderSvc {
			get {
				return _orderSvc ?? (_orderSvc = new OrderService(this.AppDbContext));
			}
			set { _orderSvc = value; }
		}
		private OrderService _orderSvc;

		[RequiredPermission(PermissionEnum.BasicPrivileges)]
		public ActionResult List() {
			ViewBag.Orders = OrderSvc.GetAll().ToList();

			return View();
		}

		[RequiredPermission(PermissionEnum.BasicPrivileges)]
		public ActionResult View_Insecure(int id) {
			ViewBag.Model = OrderSvc.GetByIdInsecure(id);

			return View("View");
		}

		[RequiredPermission(PermissionEnum.BasicPrivileges)]
		public ActionResult View_Secure(int id) {
			ViewBag.Model = OrderSvc.GetByIdSecure(id, CurrentUser);

			return View("View");
		}
	}
}