using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Model
{
	public class CustomerLock
	{
		public long id { set; get; }
		public int customerid { set; get; }
		public string objectname { set; get; }

	}
}
