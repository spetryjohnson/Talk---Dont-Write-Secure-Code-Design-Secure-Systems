using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http;
using BuildSecureSystems.Framework.Authentication;
using BuildSecureSystems.Models;
using BuildSecureSystems.Models.Orders;
using System.Net;

namespace BuildSecureSystems.Controllers {

	public class APIController : ApiController {

		[HttpGet]
		[BearerTokenAuthorization(PermissionEnum.API_ViewOrders)]
		public HttpResponseMessage Get([FromUri] GetOrderApiArgs args) {

			return Request.CreateResponse(HttpStatusCode.OK, new { });
		}
	}
}