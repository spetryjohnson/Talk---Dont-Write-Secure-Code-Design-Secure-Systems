using BuildSecureSystems.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;

namespace BuildSecureSystems.Framework.Authentication {
	public class TokenOwnerPair {
		public string ApiKey { get; set; }
		public string OwnerId { get; set; }

		public bool WasFound {
			get { return !String.IsNullOrEmpty(ApiKey) && !String.IsNullOrEmpty(OwnerId); }
		}

		public static TokenOwnerPair TryToGetFromQuerystring(string queryString) {
			var queryStringCollection = HttpUtility.ParseQueryString(queryString);

			return new TokenOwnerPair {
				OwnerId = queryStringCollection.AllKeys.Contains("ApiKeyOwner")
					? queryStringCollection["ApiKeyOwner"]
					: string.Empty,

				ApiKey = queryStringCollection.AllKeys.Contains("ApiKey")
					? queryStringCollection["ApiKey"]
					: string.Empty,
			};
		}

		public static TokenOwnerPair TryToGetFromHeaders(HttpRequestHeaders headers) {
			IEnumerable<string> apiKeyVals = new List<string>();
			headers.TryGetValues("ApiKey", out apiKeyVals);
			var apiKeyValue = apiKeyVals.Join("");

			IEnumerable<string> apiKeyOwnerVals = new List<string>();
			headers.TryGetValues("ApiKeyOwner", out apiKeyOwnerVals);
			var apiOwnerValue = apiKeyOwnerVals.Join("");

			return new TokenOwnerPair {
				ApiKey = apiKeyValue,
				OwnerId = apiOwnerValue,
			};
		}
	}
}