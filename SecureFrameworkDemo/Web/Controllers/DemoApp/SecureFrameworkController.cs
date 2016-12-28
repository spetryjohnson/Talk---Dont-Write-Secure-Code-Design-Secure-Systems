using System.Linq;
using System.Web.Mvc;
using SecureFrameworkDemo.Models;
using static Santhos.Web.Mvc.BootstrapFlashMessages.FlashControllerExtensions;
using SecureFrameworkDemo.Framework.WebPageAuthentication;

namespace SecureFrameworkDemo.Controllers {

	/// <summary>
	/// Shows a version of "SecureFeatureController" where all of the security concerns have
	/// been moved into framework code, with very little implemented as per-feature app code.
	/// like with no security at all.
	/// 
	/// 1) Requires a login (using TODO)
	/// 2) Prevents a logged-in user from seeing someone else's data (using secure data access calls
	///    plus Row Level Security)
	/// 3) Masks user SSNs (using PostSharp)
	/// 
	/// Instead of app-level security code, this controller DOES need to inherit from a specific
	/// base controller. (That's the trade-off versus using attributes)
	/// </summary>
	public class SecureFrameworkController : SecureControllerBase {

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

		[RequiredPermission(PermissionEnum.ManageOrders)]
		public ActionResult ManageOrders() {
			return View();
		}

		/// <summary>
		/// The row-level security feature means that the controller doesn't have to do any
		/// special filtering here. We can call the "insecure" service method that doesn't
		/// do any explicit checks, and the feature still ends up secure.
		/// </summary>
		public ActionResult OrderList(string userName = null) {
			var ordersUserCanAccess = OrderSvc.GetAllNoInjection(userName: userName)
				.Select(o => new SecureFrameworkOrderViewModel(o))
				.ToList();

			return View(ordersUserCanAccess);
		}

		public ActionResult OrderDetail(int id) {
			var model = new SecureFrameworkOrderViewModel(
				OrderSvc.GetByIdInsecure(id)
			);

			return View(model);
		}

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
		/// This controller action itself doesn't really have to do anything at all, which 
		/// is kind of the point :) 
		/// </summary>
		[HttpPost]
		public ActionResult ModifyOrder() {
			this.FlashSuccess("POST successful");
			return RedirectToAction("OrderList", new { });
		}

		/// <summary>
		/// Just like with a normal POST, all CSRF stuff is handled in the case controller automatically
		/// </summary>
		[HttpPost]
		public ActionResult ModifyOrderAjax(int orderId) {
			return Json(new {
				message = $"POST successful for ID: {orderId}"
			});
		}

		/// <summary>
		/// For demo purposes - MVC will happily accept incoming requests to public controller methods, even
		/// if they don't return an ActionResult. This is an example of a method returning a string that is
		/// directly accessible via the URL.
		/// </summary>
		public string SomeMethodThatShouldBePrivate() {
			return "This is a private string. It should never be displayed publicly";
		}
	}
}