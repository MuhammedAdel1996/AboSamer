using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccessLayer.Model
{
	public class Check
	{
		public int id { set; get; }
		public string description { set; get; }
		public DateTime create { set; get; }
		[ForeignKey("Customer")]
		public int customerid { set; get; }
		public virtual Customer Customer { set; get; }
		[ForeignKey("User")]
		public int? ownerid { set; get; }
		public virtual User User { set; get; }
		
		public int? useraction { set; get; }
		
		public string result { set; get; }
		public int count { set; get; }
		public bool Done { set; get; }
	}
}
