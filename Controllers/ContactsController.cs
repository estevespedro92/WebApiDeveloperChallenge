using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiDeveloperChallenge.Models;
using WebApiDeveloperChallenge.Repositories;

namespace WebApiDeveloperChallenge.Controllers
{
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  [Route("api/[controller]")]
  [ApiController]
  public class ContactsController : BaseController<Contact, ContactRepository>
  {
    private readonly ContactRepository _contactRepository;
    public ContactsController(ContactRepository repository) : base(repository)
    {
      _contactRepository = repository;
    }

    [HttpGet("skills")]
    public async Task<ActionResult<IEnumerable<ContactSkill>>> GetWithSkills()
    {
      
      return await _contactRepository.GetWithSkills();
    }
  }
}