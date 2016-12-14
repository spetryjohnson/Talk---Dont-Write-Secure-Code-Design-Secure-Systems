namespace BuildSecureSystems.Migrations {
	using System;
	using System.Collections.Generic;
	using System.Data.Entity.Migrations;
	using Microsoft.AspNet.Identity;
	using Microsoft.AspNet.Identity.EntityFramework;

	using Models;
	using Framework.Encryption;

	internal sealed class Configuration : DbMigrationsConfiguration<BuildSecureSystems.Models.ApplicationDbContext> {

		public Configuration() {
			AutomaticMigrationsEnabled = false;
		}

		protected override void Seed(BuildSecureSystems.Models.ApplicationDbContext context) {
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
			var PERM_MANAGE_USERS = new Permission { Id = (int)PermissionEnum.ManageUsers, Name = "Manage Users" };
			var PERM_VIEW_ORDERS = new Permission { Id = (int)PermissionEnum.ViewOrdersForOthers, Name = "View Orders for Others" };
			var PERM_VIEW_PAYMENT = new Permission { Id = (int)PermissionEnum.ViewCreditCard, Name = "View Credit Card #" };
			var PERM_VIEW_SSN = new Permission { Id = (int)PermissionEnum.ViewSSN, Name = "View SSN" };
			var PERM_API_VIEW_ORDERS = new Permission { Id = (int)PermissionEnum.API_ViewOrders, Name = "API: View Orders" };

			context.Permissions.AddOrUpdate(
				p => p.Id,
				PERM_BASIC_PRIVS,
				PERM_MANAGE_PRODS,
				PERM_MANAGE_USERS,
				PERM_VIEW_ORDERS,
				PERM_VIEW_PAYMENT,
				PERM_VIEW_SSN,
				PERM_API_VIEW_ORDERS
			);

			// CREATE PEOPLE
			var userManager = new UserManager<ApplicationUser>(
				new UserStore<ApplicationUser>(context)
			);

			var adminUser = new ApplicationUser {
				UserName = "admin@example.com",
				Permissions = new HashSet<Permission> { PERM_BASIC_PRIVS, PERM_MANAGE_PRODS, PERM_MANAGE_USERS, PERM_VIEW_ORDERS, PERM_VIEW_PAYMENT, PERM_VIEW_SSN },
				SSN = "111-11-1111"
			};
			userManager.Create(adminUser, "Passw0rd");

			var restrictedAdmin = new ApplicationUser {
				UserName = "restrictedAdmin@example.com",
				Permissions = new HashSet<Permission> { PERM_BASIC_PRIVS, PERM_MANAGE_PRODS, PERM_MANAGE_USERS, PERM_VIEW_ORDERS },
				SSN = "222-22-2222"
			};
			userManager.Create(restrictedAdmin, "Passw0rd");

			var user1 = new ApplicationUser {
				UserName = "user1@example.com",
				Permissions = new HashSet<Permission> { PERM_BASIC_PRIVS },
				SSN = "333-33-3333"
			};
			userManager.Create(user1, "Passw0rd");

			var user2 = new ApplicationUser {
				UserName = "user2@example.com",
				Permissions = new HashSet<Permission> { PERM_BASIC_PRIVS },
				SSN = "444-44-4444"
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
				new Order { ApplicationUser = user1, CreditCardNumber = SampleEncryptor.Encrypt("4111111111111111") },
				new Order { ApplicationUser = user2, CreditCardNumber = SampleEncryptor.Encrypt("4111111111111112") }
			);

			// CREATE API KEYS
			context.ApiKeys.AddOrUpdate(
				a => a.Id,
				new ApiKey { ApplicationUser = user1, Description = "Sample API key", Permissions = $"{PermissionEnum.API_ViewOrders.ToString()}" }
			);
		}
	}
}
