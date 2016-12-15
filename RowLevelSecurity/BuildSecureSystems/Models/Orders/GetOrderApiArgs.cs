using BuildSecureSystems.Framework.ApiAuthentication;
using System;

namespace BuildSecureSystems.Models {

	public class GetOrderApiArgs : ApiRequestBase {
		public int Id { get; set; }
	}
}