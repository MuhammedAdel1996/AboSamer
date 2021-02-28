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
        private readonly Dictionary<int, int> Time;
        public FollowUPController(ICustomerRepository CustomerRepo)
        {
            _CustomerRepo = CustomerRepo;
            Time = new Dictionary<int, int>()
        {
            { 1, 3},
            { 3, 7},
            { 7,10 },
            {10,21 },
            {21,30 },
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
        [Route("GetCustomerInfo/{id}")]
        public IActionResult GetCustomerInfo(int id)
        {
            CustomerFollowUP customerFollowUP = new CustomerFollowUP();
            var customerinfo = _CustomerRepo.GetById(id);
            if(customerinfo!=null)
            {
                customerFollowUP.address = customerinfo.address;
                customerFollowUP.id = customerinfo.id;
                customerFollowUP.name = customerinfo.name;
                customerFollowUP.ownerid = customerinfo.ownerid;
                customerFollowUP.field = customerinfo.field;
                customerFollowUP.sectorid = customerinfo.sectorid;
                customerFollowUP.created = customerinfo.created;
            }
            customerFollowUP.followUps = _CustomerRepo.GetFollowUp(id).ToList();
            return Ok(customerFollowUP);
        }
        [HttpPost]
        [Route("AddFollowUp")]
        public IActionResult AddFollowUp([FromBody] FollowUpDTO followUpDTO)
        {
            try
            {
                var customer = _CustomerRepo.GetById(followUpDTO.customerid);
                if (followUpDTO.delay.HasValue)
                {

                    if (customer != null)
                    {
                        var difference = (long)(followUpDTO.create - customer.created.AddDays(customer.count)).TotalHours;
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

                        customer.count = Time[customer.count];
                        _CustomerRepo.Update(customer);
                        _CustomerRepo.Save();
                    }
                }
                return Ok(true);
            }
            catch
            {
                return Ok(false);
            }

        }
    }
}