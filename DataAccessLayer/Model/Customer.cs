using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccessLayer.Model
{
	public class Customer
	{
		public Customer()
		{
			Employees = new HashSet<Employee>();
			FollowUps = new HashSet<FollowUp>();
			Orders = new HashSet<Order>();
		}
		public int id { set; get; }
		public string name { set; get; }
		public string address { set; get; }
		public string field { set; get; }
		[ForeignKey("Sector")]
		public int sectorid { set;get; }
		public string vacancy { set; get; }
		public string email { set; get; }
		public virtual Sector Sector { set; get; }
		[ForeignKey("User")]
		public int ownerid { set; get; }
		public virtual User User { set; get; }
		public int count { set; get; }
		public long? hours { set; get; }
		public DateTime created { set; get; }
		public virtual ICollection<Employee> Employees { set; get; }
		public virtual ICollection<FollowUp> FollowUps { set; get; }
		public virtual ICollection<Order> Orders { set; get; } 

	}
}
