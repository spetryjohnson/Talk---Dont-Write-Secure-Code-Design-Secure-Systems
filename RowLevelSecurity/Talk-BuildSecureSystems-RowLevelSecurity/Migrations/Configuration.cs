namespace Talk_BuildSecureSystems_RowLevelSecurity.Migrations {
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;
	using Microsoft.AspNet.Identity;
	using Models;
	using Models.Orders;
	using Microsoft.AspNet.Identity.EntityFramework;
	internal sealed class Configuration : DbMigrationsConfiguration<Talk_BuildSecureSystems_RowLevelSecurity.Models.ApplicationDbContext> {

		public Configuration() {
			AutomaticMigrationsEnabled = false;
		}

		protected override void Seed(Talk_BuildSecureSystems_RowLevelSecurity.Models.ApplicationDbContext context) {
			//  This method will be called after migrating to the latest version.

			//  You can use the DbSet<T>.AddOrUpdate() helper extension method 
			//  to avoid creating duplicate seed data. E.g.
			//
			//    context.People.AddOrUpdate(
			//      p => p.FullName,
			//      new Person { FullName = "Andrew Peters" },
			//      new Person { FullName = "Brice Lambson" },
			//      new Person { FullName = "Rowan Miller" }
			//    );
			//

			// CREATE PERMISSIONS
			var PERM_BASIC_PRIVS = new Permission { Id = (int)PermissionEnum.BasicPrivileges, Name = "Basic Privileges" };
			var PERM_MANAGE_PRODS = new Permission { Id = (int)PermissionEnum.ManageProducts, Name = "Manage Products" };
			var PERM_VIEW_ORDERS = new Permission { Id = (int)PermissionEnum.ViewOrdersForOthers, Name = "View Orders for Others" };

			context.Permissions.AddOrUpdate(
				p => p.Id,
				PERM_BASIC_PRIVS,
				PERM_MANAGE_PRODS,
				PERM_VIEW_ORDERS
			);

			// CREATE PEOPLE
			var userManager = new UserManager<ApplicationUser>(
				new UserStore<ApplicationUser>(context)
			);

			var adminUser = new ApplicationUser {
				UserName = "admin@example.com",
				Permissions = new HashSet<Permission> { PERM_BASIC_PRIVS, PERM_MANAGE_PRODS, PERM_VIEW_ORDERS }
			};
			userManager.Create(adminUser, "Passw0rd");

			var user1 = new ApplicationUser {
				UserName = "user1@example.com",
				Permissions = new HashSet<Permission> { PERM_BASIC_PRIVS }
			};
			userManager.Create(user1, "Passw0rd");

			var user2 = new ApplicationUser {
				UserName = "user2@example.com",
				Permissions = new HashSet<Permission> { PERM_BASIC_PRIVS }
			};
			userManager.Create(user2, "Passw0rd");

			// CREATE ORDERS
			context.Users.AddOrUpdate(
				u => u.UserName,
				adminUser,
				user1,
				user2
			);

			context.Orders.AddOrUpdate(
				o => o.Id,
				new Order { ApplicationUser = user1 },
				new Order { ApplicationUser = user2 }
			);
		}
	}
}
