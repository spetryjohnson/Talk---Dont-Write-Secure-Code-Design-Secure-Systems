using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BuildSecureSystems.Framework.WebPageAuthentication;
using BuildSecureSystems.Models;
using static Santhos.Web.Mvc.BootstrapFlashMessages.FlashControllerExtensions;

namespace BuildSecureSystems.Controllers {

	/// <summary>
	/// Demonstrates techniques for building ANTI-CSRF tokens into the framework.
	/// 
	/// In "out of the box" ASPNET, defeating CSRF requires a couple of things:
	/// 
	/// 1) Checking the referrer, and rejecting form posts from obvious 3rd parties
	/// 2) Calling Html.AntiForgeryToken() inside the form
	/// 3) Adding [ValidateAntiForgeryToken] to the action doing the work
	/// 
	/// This controller shows how we can do that WITHOUT requiring the programmer to
	/// do anything special. The approach involves these pieces:
	/// 
	/// 1) Overriding OnActionExecuting in the base controller
	/// 2) Calling Html.AntiForgeryTokenForAllForms() in the layout file
	/// 
	/// This controller itself doesn't really have to do anything at all, which is kind of
	/// the point :)  It exists primarily as a place to put this honkin' comment and to
	/// host the endpoints for the demo app.
	/// </summary>
	public class AntiCSRFController : SecuredController {

		protected OrderService OrderSvc {
			get {
				return _orderSvc ?? (_orderSvc = new OrderService(this.AppDbContext));
			}
			set { _orderSvc = value; }
		}
		private OrderService _orderSvc;

		[HttpGet]
		[RequiredPermission(PermissionEnum.BasicPrivileges)]
		public ActionResult ModifyOrder(int? id = null) {
			ViewBag.OrderList = OrderSvc.GetAll();

			if (id.HasValue) {
				ViewBag.Order = OrderSvc.GetById(id.Value);
			}

			return View();
		}

		[HttpPost]
		[RequiredPermission(PermissionEnum.BasicPrivileges)]
		[ValidateAntiForgeryToken]
		public ActionResult ModifyOrderPost() {
			// TODO: actually change something 
			this.FlashSuccess("POST successful. (NOTE: No data were changed - haven't implemented any of that yet)");
			return RedirectToAction("ModifyOrder", new {  });
		}
	}
}