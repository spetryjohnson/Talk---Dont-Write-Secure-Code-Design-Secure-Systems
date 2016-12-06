using System;
using System.Collections.Generic;
using System.Linq;
using Talk_BuildSecureSystems_RowLevelSecurity.Framework.Exceptions;

namespace Talk_BuildSecureSystems_RowLevelSecurity.Models.Orders {

	public class OrderService {
		private ApplicationDbContext _ctx;

		public OrderService(ApplicationDbContext dbContext) {
			_ctx = dbContext;
		}

		public IQueryable<Order> GetAll() {
			return _ctx.Orders.AsQueryable();
		}

		public Order GetById(int id) {
			return _ctx.Orders
				.Where(o => o.Id == id)
				.FirstOrDefault();
		}

		public Order GetByIdInsecure(int id) {
			return _ctx.Orders
				.Where(o => o.Id == id)
				.FirstOrDefault();
		}

		public Order GetByIdSecure(int orderId, ApplicationUser activeUser) {
			var order = GetByIdInsecure(orderId);

			var cannotViewOrdersForOthers = !activeUser.HasPermission(PermissionEnum.ViewOrdersForOthers);
			var isNotViewingOwnOrder = (order.ApplicationUser.UserName != activeUser.UserName);	// CAREFUL w/ EQUALITY CHECK

			if (isNotViewingOwnOrder && cannotViewOrdersForOthers) {
				throw new NotAuthorizedException($"User {activeUser.UserName} is not authorized to access order #{orderId}");
            }

			return order;
		}
	}
}