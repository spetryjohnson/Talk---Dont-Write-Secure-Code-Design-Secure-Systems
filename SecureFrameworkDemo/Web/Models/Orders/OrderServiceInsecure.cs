using System;
using System.Linq;
using SecureFrameworkDemo.Framework;

namespace SecureFrameworkDemo.Models {

	/// <summary>
	/// Example of the Order Service without any of the secure framework stuff. Used by the "insecure demo" portion.
	/// </summary>
	public class OrderServiceInsecure {
		private ApplicationDbContext _ctx;

		public OrderServiceInsecure(ApplicationDbContext dbContext) {
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