﻿using System.Web;
using System.Web.Mvc;

namespace Talk_BuildSecureSystems_MVC {
	public class FilterConfig {
		public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
			filters.Add(new HandleErrorAttribute());
		}
	}
}