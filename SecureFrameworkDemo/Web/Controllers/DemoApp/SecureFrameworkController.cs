using System.Linq;
using System.Web.Mvc;
using SecureFrameworkDemo.Models;
using static Santhos.Web.Mvc.BootstrapFlashMessages.FlashControllerExtensions;


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
	/// </summary>
	public class SecureFrameworkController : BaseController {

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

		public ActionResult OrderList() {
			ViewBag.Orders = OrderSvc.GetAll().ToList();

			return View();
		}

		public ActionResult OrderDetail(int id) {
			ViewBag.Model = OrderSvc.GetByIdInsecure(id);

			return View();
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
			// TODO: actually change something 
			this.FlashSuccess("POST successful. (NOTE: No data were changed - haven't implemented any of that yet)");
			return RedirectToAction("OrderList", new { });
		}

	}
}