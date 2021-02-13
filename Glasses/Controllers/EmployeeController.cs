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
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var result = _EmpRepo.GetAll().ToList();
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
                element.objectid = employee.id;
                element.objectname = "Employee";
                _PhonesRepo.Insert(element);
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
                element.objectid = employee.id;
                element.objectname = "Employee";
                _PhonesRepo.Insert(element);
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
            var model = _EmpRepo.GetById(id);
            return Ok(model);
        }
    }
}