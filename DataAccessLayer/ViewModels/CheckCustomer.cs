using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.ViewModels
{
	
	public class CheckCustomer
	{
		public CheckCustomer()
		{
			Checks = new List<CheckDTO>();
			Phones = new List<PhoneDTO>();
			employees = new List<EmployeeDTO>();
		}
		public int id { set; get; }
		public string name { set; get; }
		public string address { set; get; }
		public string field { set; get; }
		public int sectorid { set; get; }
		public string sectorname { set; get; }
		public int ownerid { set; get; }
		public string username { set; get; }
		public DateTime created { set; get; }
		public List<CheckDTO> Checks { set; get; }
		public List<PhoneDTO> Phones { set; get; }
		public List<EmployeeDTO> employees { set; get; }
	}
}
