﻿using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.ViewModels
{
	public class CheckResultDTO
	{
		public CheckResultDTO()
		{
			CheckResults = new List<CheckResult>();
		}
		public int id { set; get; }
		public string description { set; get; }
		public DateTime create { set; get; }
		public int customerid { set; get; }

		public int? ownerid { set; get; }
		public string useraction { set; get; }

		public string result { set; get; }
		public int count { set; get; }
		public bool Done { set; get; }
		public bool Lock { set; get; }
		public List<CheckResult> CheckResults { set; get; }
	}
}
