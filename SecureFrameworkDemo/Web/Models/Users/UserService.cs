using System;
using System.Collections.Generic;
using System.Linq;
using SecureFrameworkDemo.Framework;

namespace SecureFrameworkDemo.Models {

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

		public bool HasPermission(string id, PermissionEnum perm) {
			return _ctx.Users
				.Where(u => u.Id == id)
				.Where(u => u.Permissions.Any(p => p.Id == (int)perm))
				.Any();
		}
	}
}