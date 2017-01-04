using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SecureFrameworkDemo.Framework;
using SecureFrameworkDemo.Models;
using SecureFrameworkDemo.Framework.WebPageAuthentication;
using SecureFrameworkDemo.Framework.SecurityAudit;

namespace SecureFrameworkDemo.Controllers {

	/// <summary>
	/// This demonstrates some static analysis auditing techniques, exposed via a web endpoint
	/// simply for ease of demonstration.
	/// 
	/// In a real app, you probably shouldn't expose this via the UI. Turn it into a console app
	/// or something and invoke it from your build tool.
	/// 
	/// (For bonus points, use http://approvaltests.com/ to track the endpoint security report artifact 
	/// over time and require manual signoff any time it changes)
	/// </summary>
	public class SecurityAuditController : BaseController {

		public ActionResult Index() {
			ViewBag.ControllerEndpoints = new ControllerEndpointAudit(Assembly.GetExecutingAssembly());

			return View();
		}
	}
}