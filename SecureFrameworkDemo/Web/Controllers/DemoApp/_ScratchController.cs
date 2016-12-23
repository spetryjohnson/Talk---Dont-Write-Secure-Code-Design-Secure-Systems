using System.Linq;
using System.Web.Mvc;
using SecureFrameworkDemo.Models;
using static Santhos.Web.Mvc.BootstrapFlashMessages.FlashControllerExtensions;
using SecureFrameworkDemo.Framework.WebPageAuthentication;
using System;

namespace SecureFrameworkDemo.Controllers.Scratch {

	/// <summary>
	/// Ignore this. It exists only so that I have a place to stub out code snippets
	/// for screenshots. It doesn't do anything.
	/// </summary>
	public class ScratchController : SecureControllerBase {

		[MyCustomSecurityLogic]
		public ActionResult OrderList() {
			return View();
		}

		public ActionResult OrderDetail() {
			EnforceSecurityRules();
			return View();
		}

		public ActionResult OrderDetails() {
			return View();
		}

		private void EnforceSecurityRules() {
			throw new NotImplementedException();
		}
	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
	public class MyCustomSecurityLogicAttribute : ActionFilterAttribute {
	}
}