using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Model
{
	public class Employee
	{
		public int id { set; get; }
		public string name { set; get; }
		public string jobtitle { set; get; }
		public string email { set; get; }
		public int customerid { set; get; }
		public virtual Customer Customer { set; get; }
	}
}
