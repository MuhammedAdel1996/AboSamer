using BAL.IRepositry;
using DataAccessLayer;
using DataAccessLayer.Model;
using DataAccessLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL.Repositry
{
	public class CustomerRepository : GenerateRepositry<Customer>, ICustomerRepository
	{
		private readonly TechnicalContext taskContext;

		public CustomerRepository(TechnicalContext dbContext) : base(dbContext)
		{
			taskContext = dbContext;
		}
		public IEnumerable<int> GetCurrent()
		{
			var result = taskContext.Customer.Where(s =>s.created.AddDays(s.count).Date==DateTime.Now.Date && s.hours==null).Select(s=>s.id);
			return result;
		}
		public IEnumerable<int> GetLate()
		{
			var result = taskContext.Customer.Where(s => s.created.AddDays(s.count).AddHours((int)s.hours).Date < DateTime.Now.Date).Select(s => s.id);
			return result;
		}
		public IEnumerable<int> GetDelay()
		{
			var result = taskContext.Customer.Where(s => s.created.AddDays(s.count).AddHours((int)s.hours).Date ==DateTime.Now.Date && s.created.AddDays(s.count).AddHours((int)s.hours).Hour<=DateTime.Now.Hour&&s.hours!=null).Select(s => s.id);
			return result;
		}
		public IEnumerable<FollowUpDTO> GetFollowUp(int id)
		{
			var result = (from  e in taskContext.FollowUp join u in taskContext.Users on 
						 e.ownerid equals u.UserId where e.customerid==id
						 select new FollowUpDTO {user=u.UserName,create=e.create,customerid=e.customerid,discribtion=e.discribtion
						 ,followup=e.followup,id=e.id,order=e.order,ownerid=e.ownerid}).ToList();
			return result;
		}
		public List<int> GetNewCustomers()
		{
			var result = taskContext.Customer.Where(s => s.count < 30).Select(s => s.id).ToList();
			return result;
		}
		public CustomerFollowUP GetUserInfo(int id)
		{
			var result = (from e in taskContext.Customer join s in taskContext.Sector
						 on e.sectorid equals s.id join u in taskContext.Users
						 on e.ownerid equals u.UserId where e.id==id select (new CustomerFollowUP() {sectorid=s.id,
						 sectorname=s.Name,address=e.address,created=e.created,field=e.field,
						 id=e.id,name=e.name,ownerid=u.UserId,username=u.UserName})).FirstOrDefault();
			return result;
		}
		public OrderCustomerDTO GetUserInfoOrder(int id)
		{ 
			var result = (from e in taskContext.Customer join s in taskContext.Sector
						 on e.sectorid equals s.id join u in taskContext.Users
						 on e.ownerid equals u.UserId where e.id==id select (new OrderCustomerDTO() {sectorid=s.id,
						 sectorname=s.Name,address=e.address,created=e.created,field=e.field,
						 id=e.id,name=e.name,ownerid=u.UserId,username=u.UserName})).FirstOrDefault();
			return result;
		}
		public CheckCustomer GetUserInfoCheck(int id)
		{
				var result = (from e in taskContext.Customer join s in taskContext.Sector
						 on e.sectorid equals s.id join u in taskContext.Users
						 on e.ownerid equals u.UserId where e.id==id select (new CheckCustomer() {sectorid=s.id,
						 sectorname=s.Name,address=e.address,created=e.created,field=e.field,
						 id=e.id,name=e.name,ownerid=u.UserId,username=u.UserName})).FirstOrDefault();
			return result;
		}
		public List<EmployeeDTO> GetEmployees(int id)
		{
			var result = taskContext.Employee.Where(s => s.customerid == id).Select(
				s => new EmployeeDTO
				{
					customerid = s.customerid,
					email = s.email,
					id = s.id,
					jobtitle = s.jobtitle
				,
					name = s.name
				}).ToList();
			return result;
		}
		public List<OrderDTO> GetOrderInfo(int customerid)
		{
			var result = (from e in taskContext.Order join u in taskContext.Users
						 on e.ownerid equals u.UserId orderby e.create descending select (new OrderDTO() {useraction=e.useraction,
						 count=e.count,create=e.create,user=u.UserName,customerid=e.customerid,
						 description=e.description,Done=e.Done,id=e.id,ownerid=e.ownerid,result=e.result})).ToList();
			return result;
		}
	public List<CheckDTO> GetCheckInfo(int customerid)
		{
			var result = (from e in taskContext.Order join u in taskContext.Users
						 on e.ownerid equals u.UserId orderby e.create descending select (new CheckDTO() {useraction=e.useraction,
						 count=e.count,create=e.create,user=u.UserName,customerid=e.customerid,
						 description=e.description,Done=e.Done,id=e.id,ownerid=e.ownerid,result=e.result})).ToList();
			return result;
		}
	}
}
