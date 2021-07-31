using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Model
{
	public class OrderResult
	{
		public int id { set; get; }
		public string result { set; get; }
		public int orderid { set; get; }
		public string useraction { set; get; }
		public DateTime createdate { set; get; }
	}
}
