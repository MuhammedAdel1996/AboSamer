using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BAL.IRepositry;
using DataAccessLayer.Model;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Technical.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckController : ControllerBase
    {
        private readonly IGenericRepositry<Check> _CheckRepositry;
        private readonly ICustomerRepository _CustomerRepo;
        private readonly IPhoneRepository _PhonesRepo;
        public CheckController(IGenericRepositry<Check> CheckRepositry, ICustomerRepository CustomerRepo, IPhoneRepository PhonesRepo)
        {
            _CheckRepositry = CheckRepositry;
            _CustomerRepo = CustomerRepo;
            _PhonesRepo = PhonesRepo;
        }
        [HttpGet]
        [Route("NewOrders")]
        public IActionResult NewOrders()
        {
            var result = _CheckRepositry.GetAll().Where(s => string.IsNullOrEmpty(s.result) && s.create.AddHours(s.count) <= DateTime.Now).Select(s => s.customerid).ToList();
            return Ok(result);
        }
        [HttpGet]
        [Route("ActionOrders/{role}")]
        public IActionResult ActionOrders(int role)
        {
            var result = _CheckRepositry.GetAll().Where(s => !string.IsNullOrEmpty(s.result) && (role == 1 || s.Done == false) && s.create.AddHours(s.count) <= DateTime.Now).Select(s => s.customerid).ToList();

            return Ok(result);
        }
        [HttpPost]
        [Route("TakeAction")]
        public IActionResult TakeAction([FromBody] CheckDTO Check)
        {
            var result = _CheckRepositry.GetById(Check.id);
            if (!string.IsNullOrEmpty(Check.result) && result != null)
            {
                result.result = Check.result;
                _CheckRepositry.Update(result);
                _CheckRepositry.Save();
            }
            if (Check.Done == true && result != null)
            {
                result.Done = Check.Done;
                _CheckRepositry.Update(result);
                _CheckRepositry.Save();
            }
            if (Check.late.HasValue && result != null)
            {
                var difference = (int)(Check.late.Value - result.create).TotalHours;
                result.count = difference;
                _CheckRepositry.Update(result);
                _CheckRepositry.Save();
            }
            return Ok(true);
        }
        [HttpGet]
        [Route("Details/{id}")]
        public IActionResult Details(int id)
        {
            var ordercustomer = _CustomerRepo.GetUserInfoCheck(id);
            if (ordercustomer != null)
            {
                ordercustomer.Checks = _CheckRepositry.GetAll().Where(s => s.customerid == id).OrderByDescending(s => s.create.AddHours(s.count)).ToList();
                ordercustomer.Phones = _PhonesRepo.GetUserByObjectId("Customer", id).Select(s => new PhoneDTO { phone = s.phone, whatsapp = s.whatsapp }).ToList();
                ordercustomer.employees = _CustomerRepo.GetEmployees(id);
                foreach (var employee in ordercustomer.employees)
                {
                    employee.Phones = _PhonesRepo.GetUserByObjectId("Employee", employee.id).Select(s => new PhoneDTO { phone = s.phone, whatsapp = s.whatsapp }).ToList();
                }

            }
            return Ok(ordercustomer);
        }
    }
}