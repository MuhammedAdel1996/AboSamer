using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAL.IRepositry
{
	public interface ICustomerRepository : IGenericRepositry<Customer>
	{
		 IEnumerable<int> GetCurrent();
		IEnumerable<int> GetLate();
		IEnumerable<int> GetDelay();
		IEnumerable<FollowUp> GetFollowUp(int id);
	}
}
