using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SecureFrameworkDemo.Framework {

	/// <summary>
	/// Parses regex strings in Perl or Ruby format (e.g. "/foo/i") into .NET regex strings and
	/// associated RegexOptions values.
	/// 
	/// This is mainly a helper class used by the Regex extension methods but can be used directly
	/// if needed.
	/// </summary>
	public static class RegexParser {

		/// <summary>
		/// Accepts a pattern string like "/foo/i" and returns a Regex option containing the
		/// string pattern ("foo") and the parsed options. If the pattern does not contain the
		/// regex markers "/.../" then the entire string is returned.
		/// </summary>
		public static Regex Parse(string pattern) {
			return Parse(pattern, RegexOptions.None);
		}

		/// <summary>
		/// Accepts a pattern string like "/foo/i" and returns a Regex option containing the
		/// string pattern ("foo") and the parsed options. If the pattern does not contain the
		/// regex markers "/.../" then the entire string is returned.
		/// </summary>
		public static Regex Parse(string pattern, RegexOptions forcedOptions) {
			if (!pattern.StartsWith("/"))
				return new Regex(pattern, forcedOptions);

			var match = Regex.Match(pattern, "^/(.*)/([is]*)$");

			if (!match.Success)
				return new Regex(pattern, forcedOptions);

			Debug.Assert(match.Groups.Count >= 2
				, "Should be at least 2 groups: 1 for the whole match, one for the pattern substring");

			var patternWithoutModifier = match.Groups[1].Value;
			RegexOptions options = forcedOptions;

			// Set any additional options found as modifiers in the pattern
			if (match.Groups.Count == 3) {
				var optionsList = match.Groups[2].Value;

				if (optionsList.Contains("i"))
					options = options | RegexOptions.IgnoreCase;

				if (optionsList.Contains("s"))
					options = options | RegexOptions.Singleline;
			}

			var regex = new Regex(patternWithoutModifier, options);

			return regex;
		}
	}
}