using BAL.IRepositry;
using DataAccessLayer;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL.Repositry
{
	public class PhoneRepository : GenerateRepositry<Phones>, IPhoneRepository
	{
		private readonly TechnicalContext taskContext;

		public PhoneRepository(TechnicalContext dbContext) : base(dbContext)
		{
			taskContext = dbContext;
		}
		public IEnumerable<Phones> GetUserByObjectId(string ObjectName, int ObjectId)
		{
			var result = taskContext.Phones.Where(s => s.objectid == ObjectId && s.objectname == ObjectName);
			return result;
		}
		public void DeleteRange(List<Phones> phones)
		{
			taskContext.Phones.RemoveRange(phones);
			taskContext.SaveChanges();
		}
	}
}
