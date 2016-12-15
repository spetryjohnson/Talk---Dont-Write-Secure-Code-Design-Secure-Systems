using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuildSecureSystems.Framework {

	/// <summary>
	/// "Marker" class thrown when a security restriction is met in app code.
	/// 
	/// Web-level UI framework can catch this in a global handler and return a Not Authorized
	/// response code, if desired.
	/// </summary>
	public class NotAuthorizedException : ApplicationException {

		public NotAuthorizedException() : base() {

		}

		public NotAuthorizedException(string message) : base(message) {

		}
	}
}