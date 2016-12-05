using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Talk_BuildSecureSystems_MVC.Models;

namespace Talk_BuildSecureSystems_MVC.Services {

	public class OrderService {
		private ApplicationDbContext _ctx;

		public OrderService(ApplicationDbContext dbContext) {
			_ctx = dbContext;
		}

		public Order GetById(int id) {
			return _ctx.Orders
				.Where(o => o.Id == id)
				.FirstOrDefault();
		}
	}
}