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
    public class SectorController : ControllerBase
    {
        private readonly IGenericRepositry<Sector> _SectorRepo;
        private readonly IPhoneRepository _PhonesRepo;
        public SectorController(IGenericRepositry<Sector> SectorRepo, IPhoneRepository PhonesRepo)
        {
            _SectorRepo = SectorRepo;
            _PhonesRepo = PhonesRepo;
        }
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var result = _SectorRepo.GetAll().ToList();
            return Ok(result);
        }
        [HttpPost]
        [Route("Add")]
        public IActionResult Add([FromBody] SectorModel SectorModel)
        {
            Sector sector = new Sector();
            sector.Name = SectorModel.Name;
            _SectorRepo.Insert(sector);
            _SectorRepo.Save();

            return Ok(true);
        }
        [HttpPut]
        [Route("Edit")]
        public IActionResult Edit([FromBody] SectorModel SectorModel)
        {
            var sector = _SectorRepo.GetById(SectorModel.id);
            if (sector == null)
                return Ok(false);
            // Employee employee = new Employee();
            sector.Name = SectorModel.Name;

            _SectorRepo.Update(sector);
            _SectorRepo.Save();
           
            return Ok(true);
        }
        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var sector = _SectorRepo.GetById(id);
            if (sector == null)
                return Ok(false);
            try
            {
                _SectorRepo.Delete(id);
                _SectorRepo.Save();
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
            var model = _SectorRepo.GetById(id);
            return Ok(model);
        }
    }
}