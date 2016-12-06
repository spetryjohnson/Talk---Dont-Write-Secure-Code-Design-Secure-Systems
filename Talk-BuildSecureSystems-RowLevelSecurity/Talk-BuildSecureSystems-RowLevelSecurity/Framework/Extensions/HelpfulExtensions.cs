using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Talk_BuildSecureSystems_RowLevelSecurity.Framework.Extensions {
	public static class HelpfulExtensions {
		public static T GetAttribute<T>(this ICustomAttributeProvider member) where T : class {
			return ((typeof(T) == typeof(object))
				? (T[])member.GetCustomAttributes(false)
				: (T[])member.GetCustomAttributes(typeof(T), false))
				.FirstOrDefault();
		}
	}
}