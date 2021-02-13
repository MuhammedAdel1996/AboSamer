using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.ViewModels
{
	public class UserDTO
	{
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public bool active { set; get; }
		public int RoleId { set; get; }
	}
}
