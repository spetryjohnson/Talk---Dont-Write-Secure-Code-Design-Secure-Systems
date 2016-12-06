using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Talk_BuildSecureSystems_RowLevelSecurity.Framework.Exceptions {
	public class NotAuthorizedException : ApplicationException {

		public NotAuthorizedException() : base() {

		}

		public NotAuthorizedException(string message) : base(message) {

		}
	}
}