using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuildSecureSystems.Framework.API {

	/// <summary>
	/// Base class for WebAPI requests. Encapsulates the common data used for API request authentication.
	/// </summary>
	public class ApiRequestBase {

		/// <summary>
		/// The API key used to authorize this request.
		/// </summary>
		public string ApiKey { get; set; }

		/// <summary>
		/// The owner ID used to validate the API key. (It's not enough for keys to be unique, we
		/// also want them to be hard to guess. Requiring 2 GUIDs is pretty darn hard to guess)
		/// </summary>
		public string ApiKeyOwner { get; set; }
	}
}