using System.Threading.Tasks;
using ContactManagement.Core.Dtos;
using ContactManagement.Core.Repositories.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ContactInfoController : ControllerBase
    {
        private readonly IContactInfoRepository _contactInfoRepository;
       // private readonly IUrlHelper _urlHelper;
        public ContactInfoController(IContactInfoRepository contactInfoRepository, IUrlHelper urlHelper)
        {
            _contactInfoRepository = contactInfoRepository;
          //  _urlHelper = urlHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]ContactInfoSearchArgs args)
        {
            var contacts = await _contactInfoRepository.GetAll(args);
            
            return Ok(contacts);
        }

        [HttpGet("getcontactinfobyid")]
        public async Task<IActionResult> GetContactInfoById(int id)
        {
            var contact = await _contactInfoRepository.GetById(id);
            return Ok(contact);
        }

        [HttpPost]
        public IActionResult Post([FromForm] ContactInfoDto contact)
        {
            _contactInfoRepository.Save(contact);
            return NoContent();
        }
        [HttpPut]
        public IActionResult Put([FromForm] ContactInfoDto contact)
        {
            _contactInfoRepository.Update(contact);
            return NoContent();
        }
        [HttpDelete]
        public IActionResult Delete([FromForm] ContactInfoDto contact)
        {
            _contactInfoRepository.Delete(contact);
            return NoContent();
        }
    }
}