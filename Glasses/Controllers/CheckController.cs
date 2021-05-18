﻿using System;
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
        public CheckController(IGenericRepositry<Check> CheckRepositry, ICustomerRepository CustomerRepo, IPhoneRepository PhonesRepo, IGenericRepositry<CheckResult> CheckResultRepositry)
        {
            _CheckRepositry = CheckRepositry;
            _CustomerRepo = CustomerRepo;
            _PhonesRepo = PhonesRepo;
            _CheckResultRepositry = CheckResultRepositry;
        }
        [HttpGet]
        [Route("NewCheck")]
        public IActionResult NewOrders()
        {
            var result = _CheckRepositry.GetAll().Where(s => string.IsNullOrEmpty(s.result) && s.create.AddHours(s.count) <= DateTime.Now).Select(s => s.customerid).ToList();
            return Ok(result);
        }
        [HttpGet]
        [Route("ActionCheck/{role}")]
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
                CheckResult CheckResult = new CheckResult();
                CheckResult.orderid = Check.id;
                CheckResult.result = Check.result;
                CheckResult.useraction = Check.useraction;
                result.Lock = false;
                _CheckResultRepositry.Insert(CheckResult);
                _CheckResultRepositry.Save();
                _CheckRepositry.Update(result);
                _CheckRepositry.Save();
            }
            if (Check.Done == true && result != null)
            {
                result.Done = Check.Done;
                result.useraction = Check.useraction;
                result.Lock = false;
                _CheckRepositry.Update(result);
                _CheckRepositry.Save();
            }
            if (Check.late.HasValue && result != null)
            {
                var difference = (int)(Check.late.Value - result.create).TotalHours;
                result.count = difference;
                result.useraction = Check.useraction;
                result.Lock = false;
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
        public IActionResult CheckLock(int id)
        {
            var result = _CheckRepositry.GetById(id);
            if (result == null)
                return BadRequest(true);
            
            return Ok(result.Lock);
        }
        [HttpGet]
        [Route("SetLock/{id}")]
        public IActionResult SetLock(int id)
        {
            var result = _CheckRepositry.GetById(id);
            if (result == null)
                return BadRequest();

            result.Lock = true;
            _CheckRepositry.Update(result);
            _CheckRepositry.Save();
            return Ok(true);
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