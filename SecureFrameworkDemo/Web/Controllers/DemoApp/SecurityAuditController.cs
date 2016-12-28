﻿using System.Linq;
using System.Web.Mvc;
using System.Web;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using SecureFrameworkDemo.Models;

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
	public class SecurityAuditController : SecureControllerBase {

		public ActionResult Index() {
			ViewBag.ControllerEndpoints = GetControllerEndpointAnalysisUsingReflection();

			return View();
		}

		private List<ControllerAuditItem> GetControllerEndpointAnalysisUsingReflection() {
			var report = new List<ControllerAuditItem>();

			var bclControllerType = typeof(Controller);
			var baseControllerType = typeof(BaseController);
			var secureControllerType = typeof(SecureControllerBase);

			var controllers = Assembly.GetExecutingAssembly()
				.GetTypes()
				.Where(t => bclControllerType.IsAssignableFrom(t));

			foreach (var controller in controllers) {
				report.Add(new ControllerAuditItem {
					Type = controller,
					UsesBaseController = baseControllerType.IsAssignableFrom(controller),
					UsesSecureController = secureControllerType.IsAssignableFrom(controller)
				});
			}

			return report;
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