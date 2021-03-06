﻿using System.Linq;
using System.Web.Mvc;
using SecureFrameworkDemo.Models;
using static Santhos.Web.Mvc.BootstrapFlashMessages.FlashControllerExtensions;


namespace SecureFrameworkDemo.Controllers {

	/// <summary>
	/// The insecure part of the demo uses this controller as an example of what the code would look
	/// like with no security at all.
	/// 
	/// 1) Does not require a login
	/// 2) Does not prevent a logged-in user from seeing someone else's data
	/// 3) Does not mask user SSNs
	/// </summary>
	public class InsecureController : BaseController {

		protected OrderService OrderSvc {
			get {
				return _orderSvc ?? (_orderSvc = new OrderService(this.AppDbContext));
			}
			set { _orderSvc = value; }
		}
		private OrderService _orderSvc;

		public ActionResult Index() {
			return View();
		}

		public ActionResult ManageOrders() {
			return View();
		}

		public ActionResult OrderList(string userName = null) {
			var model = OrderSvc
				.GetAllInsecure(userName: userName)
				.Select(x => new InsecureOrderViewModel(x))
				.ToList();

			return View(model);
		}

		public ActionResult OrderDetail(int id) {
			var model = new InsecureOrderViewModel(
				OrderSvc.GetByIdInsecure(id)
			);

			return View(model);
		}

		[HttpPost]
		public ActionResult ModifyOrder() {
			// TODO: actually change something 
			this.FlashSuccess("POST successful. (NOTE: No data were changed - haven't implemented any of that yet)");
			return RedirectToAction("OrderList", new { });
		}

		[HttpPost]
		public ActionResult ModifyOrderAjax(int orderId) {
			return Json(new {
				message = $"POST successful for ID: {orderId}"
			});
		}
	}
}