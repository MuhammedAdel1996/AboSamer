using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.ViewModels
{
	public class FollowUpDTO
	{
		public int id { set; get; }
		public int customerid { set; get; }
		public int ownerid { set; get; }
		public bool followup { set; get; }
		public bool order { set; get; }
		public DateTime create { set; get; }
		public string discribtion { set; get; }
		public DateTime? delay { set; get; }
		
	}
}
