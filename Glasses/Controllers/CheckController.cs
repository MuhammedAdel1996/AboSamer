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
        private readonly IGenericRepositry<CheckResult> _CheckResultRepositry;
        private readonly ICustomerRepository _CustomerRepo;
        private readonly IPhoneRepository _PhonesRepo;
        private readonly IGenericRepositry<Lock> _LockRepositry;
        public CheckController(IGenericRepositry<Check> CheckRepositry, ICustomerRepository CustomerRepo, IPhoneRepository PhonesRepo, IGenericRepositry<CheckResult> CheckResultRepositry, IGenericRepositry<Lock> LockRepositry)
        {
            _CheckRepositry = CheckRepositry;
            _CustomerRepo = CustomerRepo;
            _PhonesRepo = PhonesRepo;
            _CheckResultRepositry = CheckResultRepositry;
            _LockRepositry = LockRepositry;
        }
        [HttpGet]
        [Route("NewCheck")]
        public IActionResult NewOrders()
        {
            var result = _CheckRepositry.GetAll().Where(s => _CheckResultRepositry.GetAll().Where(x => x.orderid == s.id).Count() == 0 && s.create.AddHours(s.count) <= DateTime.Now).Select(s => s.customerid).ToList().Distinct();
            return Ok(result);
        }
        [HttpGet]
        [Route("ActionCheck/{role}")]
        public IActionResult ActionOrders(int role)
        {
            var result = _CheckRepositry.GetAll().Where(s => _CheckResultRepositry.GetAll().Where(x => x.orderid == s.id).Count() > 0 && (role == 1 || s.Done == false) && s.create.AddHours(s.count) <= DateTime.Now).Select(s => s.customerid).ToList().Distinct();

            return Ok(result);
        }
        [HttpPost]
        [Route("TakeAction")]
        public IActionResult TakeAction([FromBody] CheckDTO Check)
        {
            var result = _CheckRepositry.GetById(Check.id);
            if (!string.IsNullOrEmpty(Check.result) && result != null)
            {
                CheckResult CheckResult = new CheckResult();
                CheckResult.orderid = Check.id;
                CheckResult.result = Check.result;
                CheckResult.useraction = Check.useraction;
                var lockresult = _LockRepositry.GetAll().Where(s => s.customerid == Check.customerid && s.objectname == "Check").FirstOrDefault();
                if (lockresult != null)
                {
                    _LockRepositry.Delete(lockresult.id);
                    _LockRepositry.Save();

                }
                _CheckResultRepositry.Insert(CheckResult);
                _CheckResultRepositry.Save();
                _CheckRepositry.Update(result);
                _CheckRepositry.Save();
            }
            if (Check.Done == true && result != null)
            {
                result.Done = Check.Done;
                result.useraction = Check.useraction;
                var lockresult = _LockRepositry.GetAll().Where(s => s.customerid == Check.customerid && s.objectname == "Check").FirstOrDefault();
                if (lockresult != null)
                {
                    _LockRepositry.Delete(lockresult.id);
                    _LockRepositry.Save();

                }
                _CheckRepositry.Update(result);
                _CheckRepositry.Save();
            }
            if (Check.late.HasValue && result != null)
            {
                if (Check.late.Value < DateTime.Now || Check.late.Value.Hour < 9 || Check.late.Value.Hour > 17)
                    return Ok("Invalid Date");
                var difference = (int)(Check.late.Value - result.create).TotalHours;
                result.count = difference;
                result.useraction = Check.useraction;
                var lockresult = _LockRepositry.GetAll().Where(s => s.customerid == Check.customerid && s.objectname == "Check").FirstOrDefault();
                if (lockresult != null)
                {
                    _LockRepositry.Delete(lockresult.id);
                    _LockRepositry.Save();

                }
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
                ordercustomer.Checks = _CustomerRepo.GetCheckInfo(id);
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
        [Route("CheckLock/{id}")]
        public IActionResult CheckLock(int id, string type)
        {
            var result = _LockRepositry.GetAll().Where(s => s.customerid == id && s.objectname == type).FirstOrDefault();
            if (result == null)
                return Ok(false);

            return Ok(true);
        }
        [HttpPost]
        [Route("SetLock")]
        public IActionResult SetLock([FromBody]Lock l)
        {
            _LockRepositry.Insert(l);
            _LockRepositry.Save();
            return Ok();
        }
        [HttpGet]
        [Route("GetResults/{id}")]
        public IActionResult GetResults(int id)
        {
            CheckResultDTO CheckResultDTO = new CheckResultDTO();
            var check = _CheckRepositry.GetById(id);
            if (check != null)
            {
                CheckResultDTO.id = check.id;
                CheckResultDTO.Lock = check.Lock;
                CheckResultDTO.ownerid = check.ownerid;
                CheckResultDTO.useraction = check.useraction;
                CheckResultDTO.Done = check.Done;
                CheckResultDTO.count = check.count;
                CheckResultDTO.create = check.create;
                CheckResultDTO.description = check.description;
                CheckResultDTO.CheckResults = _CheckResultRepositry.GetAll().Where(s => s.orderid == id).ToList();
                return Ok(CheckResultDTO);
            }
            return BadRequest();


        }
    }
}