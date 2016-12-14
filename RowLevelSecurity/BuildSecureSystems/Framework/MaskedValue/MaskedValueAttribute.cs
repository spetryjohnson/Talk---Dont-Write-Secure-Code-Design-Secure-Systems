using PostSharp.Aspects;
using PostSharp.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuildSecureSystems.Framework.MaskedValue {

	[PSerializable]
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class MaskedValueAttribute : LocationInterceptionAspect {

		public override void OnGetValue(LocationInterceptionArgs args) {
			args.ProceedGetValue();

			if (args.Value != null && args.Value is string) {
				// TODO: Perm check
				args.Value = new String('*', ((string)args.Value).Length);
			}
		}

	}
}