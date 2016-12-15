using SecureFrameworkDemo.Framework.ApiAuthentication;
using System;

namespace SecureFrameworkDemo.Models {

	public class GetOrderApiArgs : ApiRequestBase {
		public int Id { get; set; }
	}
}