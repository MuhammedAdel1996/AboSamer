using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.ViewModels
{
	public class EmployeeDTO
	{
		public EmployeeDTO()
		{
			Phones = new List<string>();
		}
		public int id { set; get; }
		public string name { set; get; }
		public string jobtitle { set; get; }
		public string email { set; get; }
		public int customerid { set; get; }
		public List<string> Phones { set; get; }
	}
}
