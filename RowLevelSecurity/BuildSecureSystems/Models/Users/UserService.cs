using System;
using System.Collections.Generic;
using System.Linq;
using BuildSecureSystems.Framework.Exceptions;

namespace BuildSecureSystems.Models {

	public class UserService {
		private ApplicationDbContext _ctx;

		public UserService(ApplicationDbContext dbContext) {
			_ctx = dbContext;
		}

		public IQueryable<ApplicationUser> GetAll() {
			return _ctx.Users.AsQueryable();
		}

		public ApplicationUser GetById(string id) {
			return _ctx.Users
				.Where(u => u.Id == id)
				.FirstOrDefault();
		}
	}
}