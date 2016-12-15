using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using SecureFrameworkDemo.Framework;

namespace SecureFrameworkDemo.Framework.ApiAuthentication {

	/// <summary>
	/// Encapsulates the two pieces of information that we need to authenticate an API request and
	/// the logic that allows them to be passed either by querystring, or in a header.
	/// </summary>
	public class ApiKeyRequestParams {
		public string ApiKey { get; set; }
		public string OwnerId { get; set; }

		public bool WasFound {
			get { return !String.IsNullOrEmpty(ApiKey) && !String.IsNullOrEmpty(OwnerId); }
		}

		public static ApiKeyRequestParams GetFromQuerystring(string queryString) {
			var queryStringCollection = HttpUtility.ParseQueryString(queryString);

			var apiKey = queryStringCollection.AllKeys.Contains("ApiKey")
				? queryStringCollection["ApiKey"]
				: string.Empty;

			var ownerId = queryStringCollection.AllKeys.Contains("ApiKeyOwner")
				? queryStringCollection["ApiKeyOwner"]
				: string.Empty;

			if (String.IsNullOrEmpty(ownerId) || String.IsNullOrEmpty(apiKey)) {
				return null;
			}
			else {
				return new ApiKeyRequestParams { ApiKey = apiKey, OwnerId = ownerId };
			}
		}

		public static ApiKeyRequestParams GetFromHeaders(HttpRequestHeaders headers) {
			IEnumerable<string> apiKeyVals = new List<string>();
			headers.TryGetValues("ApiKey", out apiKeyVals);
			var apiKey = apiKeyVals.Join("");

			IEnumerable<string> apiKeyOwnerVals = new List<string>();
			headers.TryGetValues("ApiKeyOwner", out apiKeyOwnerVals);
			var ownerId = apiKeyOwnerVals.Join("");

			if (String.IsNullOrEmpty(ownerId) || String.IsNullOrEmpty(apiKey)) {
				return null;
			}
			else {
				return new ApiKeyRequestParams { ApiKey = apiKey, OwnerId = ownerId };
			}
		}
	}
}