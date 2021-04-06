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
    public class FollowUPController : ControllerBase
    {
        private readonly ICustomerRepository _CustomerRepo;
        private readonly IPhoneRepository _PhonesRepo;
        private readonly Dictionary<int, int> Time;
        private readonly IGenericRepositry<FollowUp> _genericRepositry;
        private readonly IGenericRepositry<Order> _OrderRepositry;
        private readonly IGenericRepositry<Check> _CheckRepositry;

        public FollowUPController(ICustomerRepository CustomerRepo, IPhoneRepository PhonesRepo, IGenericRepositry<FollowUp> genericRepositry
            , IGenericRepositry<Order> OrderRepositry, IGenericRepositry<Check> CheckRepositry)
        {
            _CustomerRepo = CustomerRepo;
            _PhonesRepo = PhonesRepo;
            _genericRepositry = genericRepositry;
            _OrderRepositry = OrderRepositry;
            _CheckRepositry = CheckRepositry;
            Time = new Dictionary<int, int>()
        {
            { 1, 3},
            { 3, 10},
            { 10,25 },
            {25,30 },
            {30,30 }
        };
        }
        [HttpGet]
        [Route("GetCuurent")]
        public IActionResult GetCuurent()
        {
            var result = _CustomerRepo.GetCurrent();
            return Ok(result);
        }
        [HttpGet]
        [Route("GetLate")]
        public IActionResult GetLate()
        {
            var result = _CustomerRepo.GetLate();
            return Ok(result);
        }
        [HttpGet]
        [Route("GetDelay")]
        public IActionResult GetDelay()
        {
            var result = _CustomerRepo.GetDelay();
            return Ok(result);
        }
        [HttpGet]
        [Route("NewCustomters")]
        public IActionResult NewCustomters()
        {
            var result = _CustomerRepo.GetNewCustomers();
            return Ok(result);
        }
        [HttpGet]
        [Route("GetCustomerInfo/{id}")]
        public IActionResult GetCustomerInfo(int id)
        {
            var customerFollowUP = _CustomerRepo.GetUserInfo(id);
            if(customerFollowUP!=null)
            {
                customerFollowUP.followUps = _CustomerRepo.GetFollowUp(id).ToList();
                customerFollowUP.Phones = _PhonesRepo.GetUserByObjectId("Customer", id).Select(s => new PhoneDTO {phone=s.phone,whatsapp=s.whatsapp } ).ToList();
                customerFollowUP.employees = _CustomerRepo.GetEmployees(id);
                foreach(var employee in customerFollowUP.employees)
                {
                   employee.Phones= _PhonesRepo.GetUserByObjectId("Employee", employee.id).Select(s => new PhoneDTO { phone = s.phone, whatsapp = s.whatsapp }).ToList();
                }
            }

            return Ok(customerFollowUP);
        }
        [HttpPost]
        [Route("AddFollowUp")]
        public string AddFollowUp([FromBody] FollowUpDTO followUpDTO)
        {
            try
            {
                var customer = _CustomerRepo.GetById(followUpDTO.customerid);
                if (followUpDTO.delay.HasValue)
                {
                    if (followUpDTO.delay.Value < DateTime.Now || followUpDTO.delay.Value.Hour < 9 || followUpDTO.delay.Value.Hour > 17)
                        return "Invalid Date";
                    if (customer != null)
                    {
                        var difference = (long)(followUpDTO.create - customer.created.AddDays(customer.count).AddHours((int)customer.hours)).TotalHours;
                        customer.hours = difference;
                        _CustomerRepo.Update(customer);
                        _CustomerRepo.Save();
                    }
                }
                else
                {
                    FollowUp followUp = new FollowUp();
                    followUp.create = followUpDTO.create;
                    followUp.customerid = followUpDTO.customerid;
                    followUp.discribtion = followUpDTO.discribtion;
                    followUp.order = followUpDTO.order;
                    followUp.followup = followUpDTO.followup;
                    followUp.ownerid = followUpDTO.ownerid;
                    if (customer != null)
                    {
                        customer.hours = null;
                        customer.count = Time[customer.count];
                        _CustomerRepo.Update(customer);
                        _CustomerRepo.Save();
                        _genericRepositry.Insert(followUp);
                        _genericRepositry.Save();
                    }
                    if(followUp.order)
                    {
                        Order order = new Order();
                        order.count = 0;
                        order.customerid = customer.id;
                        order.ownerid = followUp.ownerid;
                        order.description = followUp.discribtion;
                        order.create = DateTime.Now;
                        order.Done = false;
                        _OrderRepositry.Insert(order);
                        _OrderRepositry.Save();

                    }
                    if(followUp.followup)
                    {
                        Check Check = new Check();
                        Check.count = 0;
                        Check.customerid = customer.id;
                        Check.ownerid = followUp.ownerid;
                        Check.description = followUp.discribtion;
                        Check.create = DateTime.Now;
                        Check.Done = false;
                        _CheckRepositry.Insert(Check);
                        _CheckRepositry.Save();

                    }
                }
                return "Added Done";
            }
            catch
            {
                return "Error in Add";
            }

        }
    }
}