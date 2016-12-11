using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using BuildSecureSystems.Models;

namespace BuildSecureSystems.Controllers {

	public class BaseController : Controller {

		public ApplicationUserManager UserManager {
			get {
				return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
			private set { _userManager = value; }
		}
		private ApplicationUserManager _userManager;


		public ApplicationUser CurrentUser {
			get {
				if (this._currentUser == null) {
					if (User.Identity.IsAuthenticated) {
						this._currentUser = UserManager.FindById(User.Identity.GetUserId());
					}
				}
				return this._currentUser;
			}
			set { this._currentUser = value; }
		}
		private ApplicationUser _currentUser;


		public ApplicationDbContext AppDbContext {
			get {
				return _dbContext ?? (_dbContext = new ApplicationDbContext());
			}
			set { _dbContext = value; }
		}
		private ApplicationDbContext _dbContext;
	}
}