using BuildSecureSystems.Models;
using BuildSecureSystems.Models.ApiKeys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BuildSecureSystems.Framework.Authentication {
	
	/// <summary>
	/// The ApiKey and ApiKeyOwner must both be specified by either custom header or querystring
	/// Will throw NotAuthorizedException if ApiKey/Owner are not sent, not found in the database, or it does not have the permission specified
	/// </summary>
	public class BearerTokenAuthorizationAttribute : AuthorizationFilterAttribute {
		private readonly PermissionEnum[] _permissions;

		public BearerTokenAuthorizationAttribute(params PermissionEnum[] permissions) {
			_permissions = permissions;
		}

		public override void OnAuthorization(HttpActionContext actionContext) {
			// Getting the repo here, otherwise it will be cached in this attribute and become a singleton,
			// which means the dbcontext connection and its cache will live on too.
			// In real code you might do some service location IOC stuff here.
			var apiKeySvc = new ApiKeyService(new ApplicationDbContext());

			var key = new BearerTokenModel(apiKeySvc);

			if (!key.HasPermission(actionContext.Request, _permissions)) {
				DenyRequest(actionContext, key.ErrorMessage);
				return;
			}

			base.OnAuthorization(actionContext);
		}

		/// <summary>
		/// Wrapper around setting up the response to disallow the action
		/// </summary>
		private static void DenyRequest(HttpActionContext actionContext, string responseMessage) {
			actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
			actionContext.Response.ReasonPhrase = responseMessage;
		}
	}
}