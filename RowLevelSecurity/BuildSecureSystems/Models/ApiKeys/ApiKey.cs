using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using BuildSecureSystems.Framework;

namespace BuildSecureSystems.Models {

	public class ApiKey {

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public Guid Id { get; set; }

		/// <summary>
		/// Identifies the account owner, 
		/// </summary>
		public ApplicationUser ApplicationUser { get; set; }

		public string Description { get; set; }

		/// <summary>
		/// Comma-separated list of Permission Enums. (Stored as a single field for simplicity)
		/// </summary>
		public string Permissions { get; set; }

		public List<PermissionEnum> PermissionList {
			get {
				return (Permissions ?? "").ToEnumList<PermissionEnum>();
			}
		}

		public ApiKey() {
			Id = Guid.NewGuid();
		}

		public bool HasAnyPermission(IEnumerable<PermissionEnum> permissions) {
			return this.PermissionList.Any(p => permissions.Contains(p));
		}
	}
}