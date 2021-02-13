using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.ViewModels
{
	public class SectorModel
	{
		public SectorModel()
		{
			Phones = new List<Phones>();
		}
		public int id { set; get; }
		public string Name { set; get; }
		public List<Phones> Phones { set; get; }
	}
}
