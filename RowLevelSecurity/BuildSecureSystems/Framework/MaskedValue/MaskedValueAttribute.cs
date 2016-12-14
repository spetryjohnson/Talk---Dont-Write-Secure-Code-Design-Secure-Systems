using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PostSharp.Aspects;
using PostSharp.Serialization;
using BuildSecureSystems.Framework.PostSharpAttributes;
using BuildSecureSystems.Models;

namespace BuildSecureSystems.Framework.MaskedValue {

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