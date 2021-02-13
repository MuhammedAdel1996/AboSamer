using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BAL.IRepositry;
using DataAccessLayer;
using DataAccessLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Glasses.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserRepositry genericRepository;
        private readonly IGenericRepositry<Roles> _RolesRepo;
        public UserController(IUserRepositry _genericRepository, IGenericRepositry<Roles> RolesRepo)
        {
            genericRepository = _genericRepository;
            _RolesRepo = RolesRepo;
        }
        [HttpGet("GetAll")]
        public ActionResult<IEnumerable<User>> GetAll()
        {
            return Ok(genericRepository.GetAll());
        }
        [HttpGet("GetUser/{UserId}")]
        public ActionResult GetUserById(int UserId)
        {
            User model = genericRepository.GetById(UserId);
            return Ok(model);
        }
        [HttpPut("EditUser")]
        public ActionResult EditUser([FromBody]User model)
        {
            if (ModelState.IsValid)
            {
                genericRepository.Update(model);
                genericRepository.Save();
                return Ok(model);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpDelete("Delete/{UserId}")]
        public ActionResult Delete(int UserId)
        {
            var result = genericRepository.GetById(UserId);
            if (result == null)
                return Ok(false);
            else
            {
                result.active = false;
                genericRepository.Update(result);
                genericRepository.Save();
                return Ok(true);
            }
        }
        [HttpPost("AddUser")]
        public ActionResult AddUser([FromBody]User model)
        {
            if (ModelState.IsValid)
            {
                genericRepository.Insert(model);
                genericRepository.Save();
                
            }
            return Ok(true);
        }
        [HttpPost("Login")]
        public ActionResult login([FromBody]User model)
        {
            if (ModelState.IsValid)
            {
                var responseData = genericRepository.FindUser(model);
                if (responseData != null)
                    return Ok(responseData);
                else
                    return Ok(null);
            }
            return Ok(null);
        }
        [HttpGet]
        [Route("GetRoles")]
        public IActionResult GetRoles()
        {
            var roles = _RolesRepo.GetAll();
            return Ok(roles);
        }
    }
}