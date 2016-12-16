using System.Linq;
using System.Web.Mvc;
using SecureFrameworkDemo.Models;
using static Santhos.Web.Mvc.BootstrapFlashMessages.FlashControllerExtensions;


namespace SecureFrameworkDemo.Controllers {

	/// <summary>
	/// Shows a version of "InsecureController" where the security code has been implemented
	/// as feature-level concerns.
	/// 
	/// 1) Requires a login
	/// 2) Prevents a logged-in user from seeing someone else's data
	/// 3) Masks user SSNs
	/// </summary>
	public class SecureFeatureController : BaseController {

		protected OrderService OrderSvc {
			get {
				return _orderSvc ?? (_orderSvc = new OrderService(this.AppDbContext));
			}
			set { _orderSvc = value; }
		}
		private OrderService _orderSvc;

		/// <summary>
		/// The need for ONE action in this controller to be public means that we can't add [Authorize] at the controller
		/// level and instead every other action, but this one, has to add [Authorize].
		/// </summary>
		public ActionResult Index() {
			return View();
		}

		/// <summary>
		/// Action implements manual permission check
		/// </summary>
		[Authorize]
		public ActionResult ManageOrders() {
			if (!CurrentUser.HasPermission(PermissionEnum.ManageOrders)) {
				return new HttpUnauthorizedResult();
			}

			return View();
		}

		/// <summary>
		/// Action implements its own access control checks.
		/// </summary>
		[Authorize]
		public ActionResult OrderList() {
			var allOrders = OrderSvc.GetAll();

			var filteredOrders = CurrentUser.HasPermission(PermissionEnum.ManageOrders)
				? allOrders
				: allOrders.Where(o => o.ApplicationUser.Id == CurrentUser.Id);

			var viewModels = filteredOrders.ToList()
				.Select(o => new SecureFeatureOrderViewModel(o, this.CurrentUser))
				.ToList();

			return View(viewModels);
		}

		/// <summary>
		/// Action implements its own access control checks.
		/// </summary>
		[Authorize]
		public ActionResult OrderDetail(int id) {
			var order = OrderSvc.GetByIdInsecure(id);

			var isNotOwnedByCurrentUser = order.ApplicationUser.Id != CurrentUser.Id;
			var userCannotSeeAllOrders = !CurrentUser.HasPermission(PermissionEnum.ManageOrders);

			if (isNotOwnedByCurrentUser && userCannotSeeAllOrders) {
				return new HttpUnauthorizedResult();
			}

			var model = new SecureFeatureOrderViewModel(order, CurrentUser);

			return View(model);
		}

		/// <summary>
		/// Must remember the [ValidateAntiForgeryToken] attribute or else the token, even if included, does
		/// nothing to keep us secure.
		/// </summary>
		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public ActionResult ModifyOrder() {
			// TODO: actually change something 
			this.FlashSuccess("POST successful. (NOTE: No data were changed - haven't implemented any of that yet)");
			return RedirectToAction("OrderList", new { });
		}
	}
}