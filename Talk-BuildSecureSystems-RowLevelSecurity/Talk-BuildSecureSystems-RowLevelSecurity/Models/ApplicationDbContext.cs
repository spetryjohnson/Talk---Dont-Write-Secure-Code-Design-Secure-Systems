﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Talk_BuildSecureSystems_RowLevelSecurity.Models.Orders;

namespace Talk_BuildSecureSystems_RowLevelSecurity.Models {

	public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
		public ApplicationDbContext()
			: base("DefaultConnection", throwIfV1Schema: false) {
		}

		public static ApplicationDbContext Create() {
			return new ApplicationDbContext();
		}
		public virtual DbSet<Permission> Permissions { get; set; }
		public virtual DbSet<Order> Orders { get; set; }
	}
}