namespace Talk_BuildSecureSystems_MVC.Migrations {
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;
	using Microsoft.AspNet.Identity;
	using Models;
	internal sealed class Configuration : DbMigrationsConfiguration<Talk_BuildSecureSystems_MVC.Models.ApplicationDbContext> {

		public Configuration() {
			AutomaticMigrationsEnabled = false;
		}

		protected override void Seed(Talk_BuildSecureSystems_MVC.Models.ApplicationDbContext context) {
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

			var PERM_BASIC_PRIVS = new Permission { Id = (int)PermissionEnum.BasicPrivileges, Name = "Basic Privileges" };
			var PERM_MANAGE_PRODS = new Permission { Id = (int)PermissionEnum.ManageProducts, Name = "Manage Products" };
			var PERM_VIEW_ORDERS = new Permission { Id = (int)PermissionEnum.ViewOrdersForOthers, Name = "View Orders for Others" };

			context.Permissions.AddOrUpdate(
				p => p.Id,
				PERM_BASIC_PRIVS,
				PERM_MANAGE_PRODS,
				PERM_VIEW_ORDERS
			);

			var password = new PasswordHasher().HashPassword("Passw0rd");

			context.Users.AddOrUpdate(
				u => u.UserName,
				new ApplicationUser {
					UserName = "admin@example.com",
					PasswordHash = password,
					Permissions = new HashSet<Permission> { PERM_BASIC_PRIVS, PERM_MANAGE_PRODS, PERM_VIEW_ORDERS },
					SecurityStamp = Guid.NewGuid().ToString()
				},
				new ApplicationUser {
					UserName = "user@example.com",
					PasswordHash = password,
					Permissions = new HashSet<Permission> { PERM_BASIC_PRIVS },
					SecurityStamp = Guid.NewGuid().ToString()
				}
			);
		}
	}
}
