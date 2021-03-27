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
    public class EmployeeController : ControllerBase
    {
        private readonly IGenericRepositry<Employee> _EmpRepo;
        private readonly IPhoneRepository _PhonesRepo;
        public EmployeeController(IGenericRepositry<Employee> EmpRepo, IPhoneRepository PhonesRepo)
        {
            _EmpRepo = EmpRepo;
            _PhonesRepo = PhonesRepo;
        }
        [HttpGet]
        [Route("GetAll/{CustomerId}")]
        public IActionResult GetAll(int CustomerId)
        {
            var result = _EmpRepo.GetAll().Where(s=>s.customerid==CustomerId).ToList();
            return Ok(result);
        }
        [HttpPost]
        [Route("Add")]
        public IActionResult Add([FromBody] EmployeeDTO employeeDTO)
        {
            Employee employee = new Employee();
            employee.customerid = employeeDTO.customerid;
            employee.email = employeeDTO.email;
            employee.name = employeeDTO.name;
            employee.jobtitle = employeeDTO.jobtitle;
            _EmpRepo.Insert(employee);
            _EmpRepo.Save();
            foreach(var element in employeeDTO.Phones)
            {
                _PhonesRepo.Insert(new Phones() { objectid = employee.id, objectname = "Employee", phone = element.phone,whatsapp=element.whatsapp });

                _PhonesRepo.Save();
            }
            return Ok(true);
        }
        [HttpPut]
        [Route("Edit")]
        public IActionResult Edit([FromBody] EmployeeDTO employeeDTO)
        {
            var employee = _EmpRepo.GetById(employeeDTO.id);
            if (employee == null)
                return Ok(false);
           // Employee employee = new Employee();
            employee.customerid = employeeDTO.customerid;
            employee.email = employeeDTO.email;
            employee.name = employeeDTO.name;
            employee.jobtitle = employeeDTO.jobtitle;
            _EmpRepo.Update(employee);
            _EmpRepo.Save();
            var phones = _PhonesRepo.GetUserByObjectId("Employee", employeeDTO.id).ToList();
            _PhonesRepo.DeleteRange(phones);
            foreach (var element in employeeDTO.Phones)
            {
                _PhonesRepo.Insert(new Phones() { objectid = employee.id, objectname = "Employee", phone = element.phone,whatsapp=element.whatsapp });
                _PhonesRepo.Save();
            }
            return Ok(true);
        }
        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var employee = _EmpRepo.GetById(id);
            if (employee == null)
                return Ok(false);
            try
            {
                _EmpRepo.Delete(id);
                _EmpRepo.Save();
                var phones = _PhonesRepo.GetUserByObjectId("Employee", id).ToList();
                _PhonesRepo.DeleteRange(phones);
                return Ok(true);
            }
            catch
            {
                return Ok(false);
            }
        }
        [HttpGet("GetById/{id}")]
        public ActionResult GetById(int id)
        {
            EmployeeDTO dto = new EmployeeDTO();
            var model = _EmpRepo.GetById(id);
            dto.customerid = model.customerid;
            dto.email = model.email;
            dto.name = model.name;
            dto.jobtitle = model.jobtitle;
            dto.id = model.id;
            dto.Phones= _PhonesRepo.GetUserByObjectId("Employee", id).Select(s=> new PhoneDTO {phone= s.phone,whatsapp=s.whatsapp }).ToList();
            return Ok(dto);
        }
    }
}