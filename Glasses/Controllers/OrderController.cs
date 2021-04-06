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
    public class OrderController : ControllerBase
    {
        private readonly IGenericRepositry<Order> _OrderRepositry;
        private readonly ICustomerRepository _CustomerRepo;
        private readonly IPhoneRepository _PhonesRepo;
        public OrderController(IGenericRepositry<Order> OrderRepositry, ICustomerRepository CustomerRepo, IPhoneRepository PhonesRepo)
        {
            _OrderRepositry = OrderRepositry;
            _CustomerRepo = CustomerRepo;
            _PhonesRepo = PhonesRepo;
        }
        [HttpGet]
        [Route("NewOrders")]
        public IActionResult NewOrders()
        {
            var result = _OrderRepositry.GetAll().Where(s => string.IsNullOrEmpty(s.result) && s.create.AddHours(s.count)<=DateTime.Now).Select(s => s.customerid).ToList();
            return Ok(result);
        }
        [HttpGet]
        [Route("ActionOrders/{role}")]
        public IActionResult ActionOrders(int role)
        {
            var result = _OrderRepositry.GetAll().Where(s => !string.IsNullOrEmpty(s.result) &&(role==1 || s.Done==false) && s.create.AddHours(s.count) <= DateTime.Now).Select(s => s.customerid).ToList();

            return Ok(result);
        }
        [HttpPost]
        [Route("TakeAction")]
        public IActionResult TakeAction([FromBody] OrderDTO  order)
        {
            var result = _OrderRepositry.GetById(order.id);
            if (!string.IsNullOrEmpty(order.result) && result!=null)
            {
                result.result = order.result;
                _OrderRepositry.Update(result);
                _OrderRepositry.Save();
            }
            if(order.Done==true && result!=null)
            {
                result.Done = order.Done;
                _OrderRepositry.Update(result);
                _OrderRepositry.Save();
            }
            if(order.late.HasValue&& result!=null)
            {
                var difference = (int)(order.late.Value - result.create).TotalHours;
                result.count = difference;
                _OrderRepositry.Update(result);
                _OrderRepositry.Save();
            }
            return Ok(true);
        }
        [HttpGet]
        [Route("Details/{id}")]
        public IActionResult Details(int id)
        {
            var ordercustomer = _CustomerRepo.GetUserInfoOrder(id);
            if (ordercustomer != null)
            {
                ordercustomer.Orders = _OrderRepositry.GetAll().Where(s => s.customerid == id).OrderByDescending(s => s.create.AddHours(s.count)).ToList();
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