using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccessLayer.Model
{
	public class Roles
	{
		public Roles()
		{
			Users = new HashSet<User>();
		}
		[Key]
		public int RoleId { set; get; }
		[Required]
		public string RoleName { set; get; }
		public virtual ICollection<User> Users { set; get; }
	}
}
