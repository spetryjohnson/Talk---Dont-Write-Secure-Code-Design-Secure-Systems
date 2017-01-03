using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using SecureFrameworkDemo.Controllers;
using SecureFrameworkDemo.Framework.WebPageAuthentication;

namespace SecureFrameworkDemo.Framework.SecurityAudit {

	/// <summary>
	/// Given an Assembly, examines all of the MVC Controllers and produces a report of the endpoints
	/// that exist and details about their security setup.
	/// 
	/// This can be displayed on a screen, or can be exported to a text artifact so that automated
	/// tests/audits can be written against it. (See the "SecurityAudit" project for examples)
	/// </summary>
	public class ControllerEndpointAudit {

		public List<ControllerEndpointAuditItem> Endpoints { get; private set; }

		public ControllerEndpointAudit(Assembly assembly) {
			ExamineEndpointsAndPrepareReport(assembly);
		}

		private void ExamineEndpointsAndPrepareReport(Assembly assembly) {
			Endpoints = new List<ControllerEndpointAuditItem>();

			var controllerActions = assembly
				.GetTypes()
				.Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
				.SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
				.Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
				.Select(x => new {
					ControllerType = x.DeclaringType,
					Controller = x.DeclaringType.Name.Replace("Controller", ""),    // HACK: In real code, strip this from the END of the string ONLY
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
				// but stuff in the "SecureFramework" controller uses a "secure by default" approach. In real code
				// you'd obviously want to standardize on a single authentication system.
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

				Endpoints.Add(new ControllerEndpointAuditItem {
					Controller = mvcAction.Controller,
					Action = mvcAction.Action,
					RequiresAuthentication = requiresAuthentication,
					RequiresPermission = requiredPermission
				});
			}
		}
	}
}