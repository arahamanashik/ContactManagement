using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ContactManagement.Core.Dtos;
using ContactManagement.Core.Repositories.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ContactCategoryController : ControllerBase
    {

        private readonly IContactCategoryRepository _contactCategoryRepository;
        public ContactCategoryController(IContactCategoryRepository contactCategoryRepository)
        {
            _contactCategoryRepository = contactCategoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories = await _contactCategoryRepository.GetAll();
            return Ok(categories);
        }

        [HttpGet("GetContactCategoryById")]
        public async Task<IActionResult> GetContactInfoById(int id)
        {
            var category = await _contactCategoryRepository.GetById(id);
            return Ok(category);
        }

        [HttpPost]
        public IActionResult Post([FromForm] ContactCategoryDto categoryDto)
        {
            _contactCategoryRepository.Save(categoryDto);
            return NoContent();
        }
        [HttpPut]
        public IActionResult Put([FromForm] ContactCategoryDto categoryDto)
        {
            _contactCategoryRepository.Update(categoryDto);
            return NoContent();
        }
        [HttpDelete]
        public IActionResult Delete([FromForm] ContactCategoryDto categoryDto)
        {
            _contactCategoryRepository.Delete(categoryDto);
            return NoContent();
        }
    }
}