namespace Talk_BuildSecureSystems_MVC.Models {
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity;
	using System.Linq;

	public class Permission {

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }
		public string Name { get; set; }

		/// <summary>
		/// Don't really want or need this, but EF wasn't generating many-to-many join without it
		/// and didn't feel like doing manual mapping
		/// </summary>
		public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }

		public Permission() {
			ApplicationUsers = new List<ApplicationUser>();
		}

	}
}