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
    public class CustomerController : ControllerBase
    {
        private readonly IGenericRepositry<Customer> _CustomerRepo;
        private readonly IPhoneRepository _PhonesRepo;
        public CustomerController(IGenericRepositry<Customer> CustomerRepo, IPhoneRepository PhonesRepo)
        {
            _CustomerRepo = CustomerRepo;
            _PhonesRepo = PhonesRepo;
        }
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var result = _CustomerRepo.GetAll().ToList();
            return Ok(result);
        }
        [HttpPost]
        [Route("Add")]
        public IActionResult Add([FromBody] CustomerDTO CustomerDTO)
        {
            Customer Customer = new Customer();
            Customer.name = CustomerDTO.name;
            Customer.address = CustomerDTO.address;
            Customer.field = CustomerDTO.field;
            Customer.sectorid = CustomerDTO.sectorid;
            Customer.ownerid = CustomerDTO.ownerid;
            Customer.count = 3;
            Customer.vacancy = CustomerDTO.vacancy;
            Customer.email = CustomerDTO.email;
            Customer.created = DateTime.Now;
            _CustomerRepo.Insert(Customer);
            _CustomerRepo.Save();
            foreach (var element in CustomerDTO.Phones)
            {
                _PhonesRepo.Insert(new Phones() {objectid= Customer.id,objectname= "Customer",phone=element });
                _PhonesRepo.Save();
            }
            return Ok(Customer.id);
        }
        [HttpPut]
        [Route("Edit")]
        public IActionResult Edit([FromBody] CustomerDTO CustomerDTO)
        {
            var Customer = _CustomerRepo.GetById(CustomerDTO.id);
            if (Customer == null)
                return Ok(false);

            Customer.name = CustomerDTO.name;
            Customer.address = CustomerDTO.address;
            Customer.field = CustomerDTO.field;
            Customer.sectorid = CustomerDTO.sectorid;
            Customer.ownerid = CustomerDTO.ownerid;
            Customer.vacancy = CustomerDTO.vacancy;
            Customer.email = CustomerDTO.email;
            _CustomerRepo.Update(Customer);
            _CustomerRepo.Save();
            var phones = _PhonesRepo.GetUserByObjectId("Customer", CustomerDTO.id).ToList();
            _PhonesRepo.DeleteRange(phones);
            foreach (var element in CustomerDTO.Phones)
            {
                _PhonesRepo.Insert(new Phones() { objectid = Customer.id, objectname = "Customer", phone = element });
                _PhonesRepo.Save();
            }
            return Ok(true);
        }
        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var Customer = _CustomerRepo.GetById(id);
            if (Customer == null)
                return Ok(false);
            try
            {
                _CustomerRepo.Delete(id);
                _CustomerRepo.Save();
                var phones = _PhonesRepo.GetUserByObjectId("Customer", id).ToList();
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
            CustomerDTO Dto = new CustomerDTO();
            var model = _CustomerRepo.GetById(id);
            Dto.name = model.name;
            Dto.address = model.address;
            Dto.field = model.field;
            Dto.sectorid = model.sectorid;
            Dto.ownerid = model.ownerid;
            Dto.id = model.id;
            Dto.Phones =  _PhonesRepo.GetUserByObjectId("Customer", Dto.id).Select(s => s.phone).ToList();
            return Ok(Dto);
        }
    }
}