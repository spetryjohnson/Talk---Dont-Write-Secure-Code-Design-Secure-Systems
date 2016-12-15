using SecureFrameworkDemo.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SecureFrameworkDemo.Framework.ApiAuthentication {
	
	/// <summary>
	/// Added to a WebAPI action to indicate that the action is secured using the API Key framework.
	/// Calls to the action must specify both ApiKey and ApiKeyOwner as a custom header or in the
	/// querystring. The attribute will cause the request to be rejected if those values aren't sent,
	/// aren't found in the DB, or if the key they uniquely identify does not have the stated permission.
	/// 
	/// No additional authentication or authorization is needed in the body of the method.
	/// </summary>
	public class BearerTokenAuthorizationAttribute : AuthorizationFilterAttribute {
		private readonly PermissionEnum[] _permissions;

		public BearerTokenAuthorizationAttribute(params PermissionEnum[] permissions) {
			_permissions = permissions;
		}

		public override void OnAuthorization(HttpActionContext actionContext) {
			var keyData = GetAuthParamsFromRequest(actionContext.Request);

			if (keyData == null) {
				AuthParamsNotFound(actionContext);
				return;
			}

			// Constructing svc as local var. If we make it a class instance var it will be cached in 
			// this attribute and become a singleton, which means the dbcontext connection and its 
			// cache will live on too. In real code you might do some service location IOC stuff here.
			var apiKeySvc = new ApiKeyService(new ApplicationDbContext());
			var apiKey = apiKeySvc.GetById(keyData.ApiKey, keyData.OwnerId);

			if (apiKey == null) {
				ApiKeyNotFoundOrNotAuthorized(actionContext);
				return;
			}

			if (apiKey.HasAnyPermission(_permissions) == false) {
				ApiKeyNotFoundOrNotAuthorized(actionContext);
				return;
			}

			base.OnAuthorization(actionContext);
		}

		private ApiKeyRequestParams GetAuthParamsFromRequest(HttpRequestMessage request) {
			var fromHeaders = ApiKeyRequestParams.GetFromHeaders(request.Headers);

			if (fromHeaders != null) {
				return fromHeaders;
			}

			var fromQuerystring = ApiKeyRequestParams.GetFromQuerystring(request.RequestUri.Query);

			if (fromQuerystring != null) {
				return fromQuerystring;
			}

			return null;
		}

		private static void AuthParamsNotFound(HttpActionContext actionContext) {
			actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
			actionContext.Response.ReasonPhrase = "Authentication parameters were not found in headers or querystring";
		}

		private static void ApiKeyNotFoundOrNotAuthorized(HttpActionContext actionContext) {
			actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
			actionContext.Response.ReasonPhrase = "API Key and Owner Id were invalid, or lack the necessary permissions to complete the request";
		}
	}
}