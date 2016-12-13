using BuildSecureSystems.Models;
using BuildSecureSystems.Models.ApiKeys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace BuildSecureSystems.Framework.Authentication {
	public class BearerTokenModel {

		private readonly ApiKeyService _apiKeySvc;

		public BearerTokenModel(ApiKeyService apiKeyRepo) {
			this._apiKeySvc = apiKeyRepo;
		}

		public string ErrorMessage { get; private set; }

		public bool HasPermission(TokenOwnerPair tokenHeaders, TokenOwnerPair tokenQuerystring, params PermissionEnum[] permissions) {
			TokenOwnerPair apiKeyToUse = null;
			if (tokenHeaders.WasFound) {
				apiKeyToUse = tokenHeaders;
			}
			if (tokenQuerystring.WasFound) {
				apiKeyToUse = tokenQuerystring;
			}

			return DoesApiKeyHaveAnyPermission(apiKeyToUse, permissions);
		}

		public bool HasPermission(HttpRequestMessage request, params PermissionEnum[] permissions) {
			var apiKeyToUse = GetApiKeyToUse(request);

			return DoesApiKeyHaveAnyPermission(apiKeyToUse, permissions);
		}

		private bool DoesApiKeyHaveAnyPermission(TokenOwnerPair apiKeyToCheck, params PermissionEnum[] permissions) {
			if (apiKeyToCheck == null) {
				ErrorMessage = "You must specify an ApiKey and ApiKeyOwner in the header or querystring.";
				return false;
			}

			var apiKey = _apiKeySvc.GetById(apiKeyToCheck.ApiKey, apiKeyToCheck.OwnerId);
			if (apiKey == null) {
				ErrorMessage = "API Key entry not found.";
				return false;
			}

			if (!apiKey.HasAnyPermission(permissions)) {
				ErrorMessage = "API Key does not have required permission";
				return false;
			}

			return true;
		}

		public static TokenOwnerPair GetApiKeyToUse(HttpRequestMessage request) {
			var tokenHeaders = TokenOwnerPair.TryToGetFromHeaders(request.Headers);
			var tokenQuerystring = TokenOwnerPair.TryToGetFromQuerystring(request.RequestUri.Query);

			TokenOwnerPair apiKeyToUse = null;
			if (tokenHeaders.WasFound) {
				apiKeyToUse = tokenHeaders;
			}
			if (tokenQuerystring.WasFound) {
				apiKeyToUse = tokenQuerystring;
			}

			return apiKeyToUse;
		}
	}
}