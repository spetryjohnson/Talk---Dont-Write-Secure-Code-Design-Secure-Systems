using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SecureFrameworkDemo.Framework;
using SecureFrameworkDemo.Models;
using SecureFrameworkDemo.Framework.WebPageAuthentication;

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
			ViewBag.ControllerEndpoints = GetControllerEndpointAnalysisUsingReflection();

			return View();
		}

		private List<ControllerEndpointAuditItem> GetControllerEndpointAnalysisUsingReflection() {
			var report = new List<ControllerEndpointAuditItem>();

			var controllerActions = Assembly.GetExecutingAssembly()
				.GetTypes()
				.Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
				.SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
				.Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
				.Select(x => new {
					ControllerType = x.DeclaringType,
					Controller = x.DeclaringType.Name.Replace("Controller", ""),	// In real code, strip this from the END of the string ONLY
					Action = x.Name,
					ReturnType = x.ReturnType.Name,
					Attributes = x.GetCustomAttributes(),
					AttributeNames = x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", "")).Join(", "),
					IsSecureController = typeof(SecureControllerBase).IsAssignableFrom(x.DeclaringType)
				})
				.OrderBy(x => x.Controller).ThenBy(x => x.Action)
				.ToList();

			foreach (var mvcAction in controllerActions) {
				// DEMO HACK: Ignoring the "scratch" controller plus the stuff inherited from the MVC starter app
				// that I haven't cleaned out yet
				if (mvcAction.Controller.IsIn("Scratch", "Account", "Base", "Manage")) {
					continue;
				}

				// DEMO HACK: stuff in the "Insecure" and "SecureFeature" controllers is secured by [Authorize],
				// but stuff in the "SecureFramework" controller uses a "secure by default" approach
				bool requiresAuthentication;
				if (mvcAction.IsSecureController) {
					requiresAuthentication = SecureControllerBase.PathRequiresAuthentication(
						$"{mvcAction.Controller}/{mvcAction.Action}"
					);
				}
				else {
					requiresAuthentication = mvcAction.Attributes.Any(a => a is AuthorizeAttribute);
				}

				var requiredPermission = mvcAction.Attributes
					.Where(a => a is RequiredPermissionAttribute)
					.Cast<RequiredPermissionAttribute>()
					.FirstOrDefault()
					?.Permission;

				report.Add(new ControllerEndpointAuditItem {
					Controller = mvcAction.Controller,
					Action = mvcAction.Action,
					RequiresAuthentication = requiresAuthentication,
					RequiresPermission = requiredPermission
				});
			}

			return report;
		}

		private IEnumerable<Type> GetMvcControllerTypes() {
			return Assembly.GetExecutingAssembly()
				.GetTypes()
				.Where(t => typeof(Controller).IsAssignableFrom(t));
		}

		private IEnumerable<MethodInfo> GetMvcEndpoints(Type controllerType) {
			Contract.Requires(typeof(Controller).IsAssignableFrom(controllerType));

			return controllerType
				.GetMethods()
				.Where(m => m.IsPublic && !m.IsDefined(typeof(NonActionAttribute)));
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