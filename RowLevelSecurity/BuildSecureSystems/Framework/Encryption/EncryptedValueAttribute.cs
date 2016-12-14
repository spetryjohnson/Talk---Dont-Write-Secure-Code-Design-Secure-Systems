using PostSharp.Aspects;
using PostSharp.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuildSecureSystems.Framework.Encryption {

	[PSerializable]
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class EncryptedValueAttribute : LocationInterceptionAspect {

		public override void OnSetValue(LocationInterceptionArgs args) {
			if (args.Value != null && args.Value is string) {
				args.Value = SampleEncryptor.Encrypt((string)args.Value);
			}

			args.ProceedSetValue();
		}

		public override void OnGetValue(LocationInterceptionArgs args) {
			args.ProceedGetValue();

			if (args.Value != null && args.Value is string) {
				args.Value = SampleEncryptor.Decrypt((string)args.Value);
			}
		}

	}
}