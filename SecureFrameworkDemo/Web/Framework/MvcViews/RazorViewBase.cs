using SecureFrameworkDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SecureFrameworkDemo.Framework.MvcViews {

	/// <summary>
	/// Base class for Razor views. Exposes some common infrastructure stuff that is passed via ViewData.
	/// </summary>
	public abstract class RazorViewBase<TModel> : WebViewPage<TModel> where TModel : class {

		public ApplicationUser CurrentUser => this.ViewData["Framework_CurrentUser"] as ApplicationUser;
	}
}