using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace SecureFrameworkDemo.Framework.WebPageAuthentication {
	
	/// <summary>
	/// Custom web.config configuration section. 
	/// </summary>
	public class CustomAuthenticationSection : ConfigurationSection {

		/// <summary>
		/// Returns a collection of <see cref="PublicPathElement"/> objects containing regexes 
		/// describing URL patterns that are accessible without a login.
		/// </summary>
		[ConfigurationProperty("pathsAccessibleWithoutLogin", IsDefaultCollection = true)]
		public PublicPathElementCollection PathsAccessibleWithoutLogin {
			get { return (PublicPathElementCollection)this["pathsAccessibleWithoutLogin"]; }
			set { this["pathsAccessibleWithoutLogin"] = value; }
		}

		/// <summary>
		/// Given a path, returns TRUE if it matches any of the defined Path regexs. 
		/// </summary>
		public bool AllowsAnonymousAccess(string requestPath) {
			foreach (PublicPathElement path in this.PathsAccessibleWithoutLogin) {
				if (requestPath.MatchesRegex(path.Regex, RegexOptions.IgnoreCase)) {
					return true;
				}
			}

			return false;
		}
	}

	/// <summary>
	/// A collection of <see cref="PublicPathElement"/> objects describing all of the URLs that can be accessed
	/// anonymously.
	/// </summary>
	[ConfigurationCollection(typeof(PublicPathElement))]
	public class PublicPathElementCollection : ConfigurationElementCollection {

		public void Add(PublicPathElement path) {
			BaseAdd(path);
		}

		public void Add(string regex) {
			BaseAdd(
				new PublicPathElement { Regex = regex }
			);
		}

		protected override ConfigurationElement CreateNewElement() {
			return new PublicPathElement();
		}

		protected override object GetElementKey(ConfigurationElement element) {
			return ((PublicPathElement)element).Regex;
		}
	}


	/// <summary>
	/// Represents a configuration element describing a publicly accessible URL pattern.
	/// </summary>
	public class PublicPathElement : ConfigurationElement {

		/// <summary>
		/// A pattern that is applied against a requested URL to determine if it can be accessed anonymously
		/// </summary>
		[ConfigurationProperty("regex", IsRequired = true)]
		public string Regex {
			get { return (string)base["regex"]; }
			set { base["regex"] = value; }
		}
	}
}