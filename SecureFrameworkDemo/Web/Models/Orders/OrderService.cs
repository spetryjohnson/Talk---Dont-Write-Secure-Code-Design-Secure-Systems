using System;
using System.Linq;
using SecureFrameworkDemo.Framework;
using System.Collections;
using System.Collections.Generic;

namespace SecureFrameworkDemo.Models {

	public class OrderService {
		private ApplicationDbContext _ctx;

		public OrderService(ApplicationDbContext dbContext) {
			_ctx = dbContext;
		}

		public IList<Order> GetAllInsecure(string userName = null) {
			var query = "SELECT * FROM dbo.Orders";

			if (userName.IsNotNullOrEmpty()) {
				query += @"
					inner	join dbo.AspNetUsers u on Orders.ApplicationUser_Id = u.Id 
					where	u.UserName like '%" + userName + "%'";
			}

			return _ctx.Orders.SqlQuery(query).ToList();
		}

		/// <summary>
		/// A "Get All" method that is secure from SQL Injection, but does NOT implement
		/// any access control checks
		/// </summary>
		public IList<Order> GetAllNoInjection(string userName = null) {
			var allOrders = _ctx.Orders.AsQueryable();

			if (userName.IsNotNullOrEmpty()) {
				allOrders = allOrders.Where(o => o.ApplicationUser.UserName.Contains(userName));
			}

			return allOrders.ToList();
		}

		/// <summary>
		/// A "Get All" method that is secure from SQL Injection, and also implements
		/// access control permission checks.
		/// </summary>
		public IList<Order> GetAllSecure(ApplicationUser user, string userName = null) {
			var allOrders = _ctx.Orders.AsQueryable();

			if (userName.IsNotNullOrEmpty()) {
				allOrders = allOrders.Where(o => o.ApplicationUser.UserName.Contains(userName));
			}

			var cannotViewOrdersForOthers = !user.HasPermission(PermissionEnum.ManageOrders);

			if (cannotViewOrdersForOthers) {
				allOrders = allOrders.Where(o => o.ApplicationUser.UserName != user.UserName);
			}

			return allOrders.ToList();
		}

		/// <summary>
		/// Returns a single Order, does not perform access control checks
		/// </summary>
		public Order GetByIdInsecure(int id) {
			return _ctx.Orders
				.Where(o => o.Id == id)
				.FirstOrDefault();
		}

		/// <summary>
		/// Returns a single Order, DOES perform access control checks
		/// </summary>
		public Order GetByIdSecure(int orderId, ApplicationUser user) {
			var order = GetByIdInsecure(orderId);

			var cannotViewOrdersForOthers = !user.HasPermission(PermissionEnum.ManageOrders);
			var isNotViewingOwnOrder = (order.ApplicationUser.UserName != user.UserName);	

			if (isNotViewingOwnOrder && cannotViewOrdersForOthers) {
				throw new NotAuthorizedException($"User {user.UserName} is not authorized to access order #{orderId}");
            }

			return order;
		}

		public Order GetById(int id) {
			return _ctx.Orders
				.Where(o => o.Id == id)
				.FirstOrDefault();
		}

		public IList<Order> GetBySearching(string customerName) {
			return _ctx.Orders
				.Where(o => o.ApplicationUser.UserName.Contains(customerName))
				.ToList();
		}
	}
}