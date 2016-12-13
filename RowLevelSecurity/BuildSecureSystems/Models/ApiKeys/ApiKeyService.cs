using System;
using System.Collections.Generic;
using System.Linq;
using BuildSecureSystems.Framework.Exceptions;
using BuildSecureSystems.Models.Orders;

namespace BuildSecureSystems.Models.ApiKeys {

	public class ApiKeyService {
		private ApplicationDbContext _ctx;

		public ApiKeyService(ApplicationDbContext dbContext) {
			_ctx = dbContext;
		}

		public ApiKey GetById(string apiKey, string ownerId) {
			return _ctx.ApiKeys
				.Where(a => a.Id.ToString() == apiKey)
				.Where(a => a.ApplicationUser.Id == ownerId)
				.FirstOrDefault();
		}
	}
}