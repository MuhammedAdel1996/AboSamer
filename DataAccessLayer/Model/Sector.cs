using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Model
{
	public class Sector
	{
		public Sector()
		{
			Customers = new HashSet<Customer>();
		}
		public int id { set; get; }
		public string Name { set; get; }
		public virtual ICollection<Customer> Customers { set; get; }
	}
}
