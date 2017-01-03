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

		private string GetControllerEndpointAnalysisUsingRoslyn() {
			var controllerPath = @"C:\GitHub\Talk---Dont-Write-Secure-Code-Design-Secure-Systems\SecureFrameworkDemo\Web\Controllers\DemoApp\SecureFrameworkController.cs";
			using (var stream = System.IO.File.OpenRead(controllerPath)) {
				var tree = CSharpSyntaxTree.ParseText(SourceText.From(stream), path: controllerPath);
				var root = tree.GetRoot();

				ClassDeclarationSyntax controller = root.DescendantNodes()
					.OfType<ClassDeclarationSyntax>()
					.First();

				var methods =
					from m in root.DescendantNodes().OfType<MethodDeclarationSyntax>()
					where m.Modifiers.ToString().Contains("public")
					select m;

				string output = "";

				foreach (var m in methods) {
					var attr = m.AttributeLists.SelectMany(x => x.Attributes).Where(a => a.Name.ToString().Contains("RequiredPermission"))?.FirstOrDefault();

					if (attr == null)
						continue;

					var perm = attr.ArgumentList.Arguments.First();

					output += m.Identifier + ": " + perm.ToString();
				}

				return output;
			}
		}

		private bool IsController(SyntaxNode root, string args) {
			bool isValid = false;

			try {
				isValid =
					root.DescendantNodes()
						.OfType<BaseTypeSyntax>().First().ToString()
						.Equals("ApiController")
					||
					root.DescendantNodes()
						.OfType<BaseTypeSyntax>().First().ToString()
						.Equals("Controller");

			}
			catch {

			}

			return isValid;
		}
	}
}