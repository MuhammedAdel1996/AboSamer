using DataAccessLayer.Model;
using DataAccessLayer.ViewModels;
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
		IEnumerable<FollowUpDTO> GetFollowUp(int id);
		CustomerFollowUP GetUserInfo(int id);
		 List<EmployeeDTO> GetEmployees(int id);
		OrderCustomerDTO GetUserInfoOrder(int id);
		CheckCustomer GetUserInfoCheck(int id);
		List<int> GetNewCustomers();
		List<OrderDTO> GetOrderInfo(int customerid);
		List<CheckDTO> GetCheckInfo(int customerid);
	}
}
