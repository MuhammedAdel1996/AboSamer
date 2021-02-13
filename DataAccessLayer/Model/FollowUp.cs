using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccessLayer.Model
{
	public class FollowUp
	{
		public int id { set; get; }
		[ForeignKey("Customer")]
		public int customerid { set; get; }
		public virtual Customer Customer { set; get; }
		[ForeignKey("User")]
		public int ownerid { set; get; }
		public virtual User User { set; get; }
		public bool followup { set; get; }
		public bool order { set; get; }
		public DateTime create { set; get; }
		public string discribtion { set; get; }

	}
}
