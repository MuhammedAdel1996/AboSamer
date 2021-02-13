using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAL.IRepositry
{
	public interface IPhoneRepository : IGenericRepositry<Phones>
	{
		IEnumerable<Phones> GetUserByObjectId(string ObjectName, int ObjectId);
		void DeleteRange(List<Phones> phones)
	}
}
