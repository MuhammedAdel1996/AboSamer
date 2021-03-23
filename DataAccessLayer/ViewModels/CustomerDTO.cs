using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.ViewModels
{
	public class CustomerDTO
	{
		public CustomerDTO()
		{
			Phones = new List<string>();
		}
		public int id { set; get; }
		public string name { set; get; }
		public string address { set; get; }
		public string field { set; get; }
		public int sectorid { set; get; }
		public int ownerid { set; get; }
		public int count { set; get; }
		public string vacancy { set; get; }
		public string email { set; get; }
		public long? hours { set; get; }
		public List<string> Phones { set; get; }
	}
}
