using System;
using System.Collections.Generic;
using System.Linq;

namespace Talk_BuildSecureSystems_MVC.Models.Orders {

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
	}
}