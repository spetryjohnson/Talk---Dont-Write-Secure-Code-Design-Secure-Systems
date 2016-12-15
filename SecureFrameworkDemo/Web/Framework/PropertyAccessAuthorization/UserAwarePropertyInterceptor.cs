using SecureFrameworkDemo.Models;
using Microsoft.AspNet.Identity;
using PostSharp.Aspects;
using PostSharp.Serialization;
using System;
using System.Threading;

namespace SecureFrameworkDemo.Framework.PropertyAccessAuthorization {

	/// <summary>
	/// Base class for PostSharp aspects that provides a UserService and the identity of the current user.
	/// </summary>
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