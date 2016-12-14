using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using BuildSecureSystems.Models;

namespace BuildSecureSystems.Models {

	public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
		public ApplicationDbContext()
			: base("DefaultConnection", throwIfV1Schema: false) {
		}

		public static ApplicationDbContext Create() {
			return new ApplicationDbContext();
		}
		public virtual DbSet<Permission> Permissions { get; set; }
		public virtual DbSet<Order> Orders { get; set; }
		public virtual DbSet<ApiKey> ApiKeys { get; set; }
	}
}