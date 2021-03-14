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
			var result = taskContext.Customer.Where(s => s.created.AddDays(s.count).Date < DateTime.Now.Date && s.hours==null).Select(s => s.id);
			return result;
		}
		public IEnumerable<int> GetDelay()
		{
			var result = taskContext.Customer.Where(s => s.created.AddDays(s.count).Date ==DateTime.Now.Date && s.created.AddDays(s.count).AddHours((int)s.hours).Hour<=DateTime.Now.Hour&&s.hours!=null).Select(s => s.id);
			return result;
		}
		public IEnumerable<FollowUp> GetFollowUp(int id)
		{
			var result = taskContext.FollowUp.Where(s => s.customerid == id);
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
	}
}
