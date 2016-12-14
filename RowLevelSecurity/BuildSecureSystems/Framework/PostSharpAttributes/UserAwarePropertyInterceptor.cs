using BuildSecureSystems.Models;
using Microsoft.AspNet.Identity;
using PostSharp.Aspects;
using PostSharp.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace BuildSecureSystems.Framework.PostSharpAttributes {

	[PSerializable]
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class UserAwarePropertyInterceptor : LocationInterceptionAspect {

		public UserService UserService {
			get {
				return _userSvc ?? (_userSvc = new UserService(new ApplicationDbContext()));
			}
			private set { _userSvc = value; }
		}
		private UserService _userSvc;

		public string GetCurrentUserId() {
			var currentIdentity = Thread.CurrentPrincipal.Identity;

			return (currentIdentity.IsAuthenticated)
				? currentIdentity.GetUserId()
				: (string)null;
		}

		public bool IsUserLoggedIn() {
			return Thread.CurrentPrincipal.Identity.IsAuthenticated;
		}

		public bool UserIsLoggedInAndHasPermission(PermissionEnum perm) {
			if (IsUserLoggedIn() == false)
				return false;

			return UserService.HasPermission(GetCurrentUserId(), perm);
		}

		public UserAwarePropertyInterceptor() {
		}
	}
}