using System;
using PostSharp.Aspects;
using PostSharp.Serialization;
using SecureFrameworkDemo.Models;

namespace SecureFrameworkDemo.Framework.PropertyAccessAuthorization {

	/// <summary>
	/// Aspect that is added to a C# property to "mask" that property unless the current
	/// user has a specific permission.
	/// 
	/// This is a simplified concept for my demo. Real-world code might want to also collect
	/// a custom mask rather than just replacing every character with "*". Could be done w/ a 
	/// regex that specifies the stuff to be replaced, and replacement string
	///	   
	/// Note that this will also prevent other C# logic from accessing the variable. For that
	/// reason, this may be better off when applied to a view model and not a domain model.
	/// I'm not using view models in the demo for simplicitly, but would certainly use in prod.
	/// </summary>
	[PSerializable]
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class MaskedValueAttribute : UserAwarePropertyInterceptor {
		PermissionEnum RequiredPerm;

		public MaskedValueAttribute(PermissionEnum permToSeeUnmasked) {
			RequiredPerm = permToSeeUnmasked;
		}

		public override void OnGetValue(LocationInterceptionArgs args) {
			args.ProceedGetValue();

			var isMaskable = (args.Value != null) && (args.Value is string);

			if (!isMaskable)
				return;

			if (!UserIsLoggedInAndHasPermission(RequiredPerm)) { 
				args.Value = new String('*', ((string)args.Value).Length);
			}
		}
	}
}