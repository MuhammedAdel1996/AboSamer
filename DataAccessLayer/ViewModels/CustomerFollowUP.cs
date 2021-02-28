using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.ViewModels
{
	public class CustomerFollowUP
	{
		public CustomerFollowUP()
		{
			followUps = new List<FollowUp>();
		}
		public int id { set; get; }
		public string name { set; get; }
		public string address { set; get; }
		public string field { set; get; }
		public int sectorid { set; get; }
		public int ownerid { set; get; }
		public DateTime created { set; get; }
		public List<FollowUp> followUps { set; get; }
	}
}
