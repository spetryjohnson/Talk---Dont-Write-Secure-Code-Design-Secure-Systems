using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using SecureFrameworkDemo.Framework.ApiAuthentication;
using SecureFrameworkDemo.Models;

namespace SecureFrameworkDemo.Controllers {

	public class APIController : ApiController {

		[HttpGet]
		[BearerTokenAuthorization(PermissionEnum.API_ViewOrders)]
		public HttpResponseMessage Get([FromUri] GetOrderApiArgs args) {
			// magic happens here
			return Request.CreateResponse(
				HttpStatusCode.OK, 
				new { }
			);
		}
	}
}