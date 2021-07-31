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
        private readonly IGenericRepositry<OrderResult> _OrderResultRepositry;
        private readonly ICustomerRepository _CustomerRepo;
        private readonly IPhoneRepository _PhonesRepo;
        private readonly IGenericRepositry<CustomerLock> _LockRepositry;
        public OrderController(IGenericRepositry<Order> OrderRepositry, ICustomerRepository CustomerRepo, IPhoneRepository PhonesRepo, IGenericRepositry<OrderResult> OrderResultRepositry, IGenericRepositry<CustomerLock> LockRepositry)
        {
            _OrderRepositry = OrderRepositry;
            _CustomerRepo = CustomerRepo;
            _PhonesRepo = PhonesRepo;
            _OrderResultRepositry = OrderResultRepositry;
            _LockRepositry = LockRepositry;

        }
        [HttpGet]
        [Route("NewOrders")]
        public IActionResult NewOrders()
        {
            var result = _OrderRepositry.GetAll().Where(s => _OrderResultRepositry .GetAll().Where(x=>x.orderid==s.id).Count()==0&& s.create.AddHours(s.count)<=DateTime.Now).Select(s => s.customerid).ToList().Distinct();
            return Ok(result);
        }
        [HttpGet]
        [Route("ActionOrders/{role}")]
        public IActionResult ActionOrders(int role)
        {
            var result = _OrderRepositry.GetAll().Where(s => _OrderResultRepositry.GetAll().Where(x => x.orderid == s.id).Count() > 0 && (role==1 || s.Done==false) && s.create.AddHours(s.count) <= DateTime.Now).Select(s => s.customerid).ToList().Distinct();

            return Ok(result);
        }
        [HttpPost]
        [Route("TakeAction")]
        public IActionResult TakeAction([FromBody] OrderDTO  order)
        {
            var result = _OrderRepositry.GetById(order.id);
            if (!string.IsNullOrEmpty(order.result) && result != null)
            {
                OrderResult orderResult = new OrderResult();
                orderResult.orderid = order.id;
                orderResult.result = order.result;
                orderResult.useraction = order.useraction;
                var lockresult = _LockRepositry.GetAll().Where(s => s.customerid == result.customerid && s.objectname == "Order").FirstOrDefault();
                if(lockresult !=null)
                {
                    _LockRepositry.Delete(lockresult.id);
                    _LockRepositry.Save();

                }
                _OrderResultRepositry.Insert(orderResult);
                _OrderResultRepositry.Save();
                _OrderRepositry.Update(result);
                _OrderRepositry.Save();
            }
            if(order.Done==true && result!=null)
            {
                result.Done = order.Done;
                result.useraction = order.useraction;
                var lockresult = _LockRepositry.GetAll().Where(s => s.customerid == result.customerid && s.objectname == "Order").FirstOrDefault();
                if (lockresult != null)
                {
                    _LockRepositry.Delete(lockresult.id);
                    _LockRepositry.Save();

                }
                _OrderRepositry.Update(result);
                _OrderRepositry.Save();
            }
            if(order.late.HasValue&& result!=null)
            {
                if (order.late.Value < DateTime.Now || order.late.Value.Hour < 9 || order.late.Value.Hour > 17)
                    return Ok("Invalid Date");

                var difference = (int)(order.late.Value - result.create).TotalHours;
                result.count = difference;
                result.useraction = order.useraction;
                var lockresult = _LockRepositry.GetAll().Where(s => s.customerid == result.customerid && s.objectname == "Order").FirstOrDefault();
                if (lockresult != null)
                {
                    _LockRepositry.Delete(lockresult.id);
                    _LockRepositry.Save();

                }
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

                ordercustomer.Orders = _CustomerRepo.GetOrderInfo(id);
                ordercustomer.Phones = _PhonesRepo.GetUserByObjectId("Customer", id).Select(s => new PhoneDTO { phone = s.phone, whatsapp = s.whatsapp }).ToList();
                ordercustomer.employees = _CustomerRepo.GetEmployees(id);
                foreach (var employee in ordercustomer.employees)
                {
                    employee.Phones = _PhonesRepo.GetUserByObjectId("Employee", employee.id).Select(s => new PhoneDTO { phone = s.phone, whatsapp = s.whatsapp }).ToList();
                }
              
            }
            return Ok(ordercustomer);
        }
        [HttpGet]
        [Route("CheckLock/{id}/{type}")]
        public IActionResult CheckLock(int id, string type)
        {
            var result = _LockRepositry.GetAll().Where(s => s.customerid == id && s.objectname == type).FirstOrDefault();
            if (result == null)
                return Ok(false);

            return Ok(true);
        }
        [HttpPost]
        [Route("SetLock")]
        public IActionResult SetLock([FromBody]CustomerLock l)
        {
            _LockRepositry.Insert(l);
            _LockRepositry.Save();
            return Ok();
        }
        [HttpGet]
        [Route("GetResults/{id}")]
        public IActionResult GetResults(int id)
        {
            ResultOrderDTO resultOrderDTO = new ResultOrderDTO();
            var order = _OrderRepositry.GetById(id);
            if(order!=null)
            {
                resultOrderDTO.id = order.id;
                resultOrderDTO.Lock = order.Lock;
                resultOrderDTO.ownerid = order.ownerid;
                resultOrderDTO.useraction = order.useraction;
                resultOrderDTO.Done = order.Done;
                resultOrderDTO.count = order.count;
                resultOrderDTO.create = order.create;
                resultOrderDTO.description = order.description;
                resultOrderDTO.orderResults = _OrderResultRepositry.GetAll().Where(s => s.orderid == id).ToList();
                return Ok(resultOrderDTO);
            }
            return BadRequest();

          
        }

    }
}