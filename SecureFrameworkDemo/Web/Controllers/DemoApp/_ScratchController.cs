using System.Linq;
using System.Web.Mvc;
using SecureFrameworkDemo.Models;
using static Santhos.Web.Mvc.BootstrapFlashMessages.FlashControllerExtensions;
using SecureFrameworkDemo.Framework.WebPageAuthentication;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;

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

		[RequiredPermission(PermissionEnum.BasicPrivileges)]
		public ActionResult SensitiveAction() {
			EnforceSecurityRules();
			return View();
		}

		[RequiredPermission(PermissionEnum.BasicPrivileges)]
		public ActionResult Index() {
			return View();
		}

		[RequiredPermission(PermissionEnum.ManageOrders)]
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