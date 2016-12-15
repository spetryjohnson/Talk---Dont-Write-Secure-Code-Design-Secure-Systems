using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using BuildSecureSystems.Framework.ApiAuthentication;
using BuildSecureSystems.Models;

namespace BuildSecureSystems.Controllers {

	public class APIController : ApiController {

		[HttpGet]
		[BearerTokenAuthorization(PermissionEnum.API_ViewOrders)]
		public HttpResponseMessage Get([FromUri] GetOrderApiArgs args) {

			return Request.CreateResponse(HttpStatusCode.OK, new { });
		}
	}
}