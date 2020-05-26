using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiDeveloperChallenge.Models;
using WebApiDeveloperChallenge.Repositories;
using WebApiDeveloperChallenge.Representations;

namespace WebApiDeveloperChallenge.Controllers
{
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  [Route("api/[controller]")]
  [ApiController]
  public class ContactsController : ControllerBase
  {
    private readonly ContactRepository _repository;
    private readonly IMapper _mapper;
    public ContactsController(ContactRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    /// <summary>
    /// Get lists from contact (from user)
    /// </summary>
    /// <param name="isSkillsIncluded">Indicate if skills must be returned</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContactSkillRepresentation>>> Get(bool isSkillsIncluded)
    {
      var contacts = await _repository.Get(isSkillsIncluded);
      return contacts.Select(c => _mapper.Map<ContactSkillRepresentation>(c)).ToList();
    }

    
    /// <summary>
    /// Get contact (from user)
    /// </summary>
    /// <param name="id"></param>
    /// <param name="isSkillsIncluded">Indicate if skills must be returned</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public virtual async Task<ActionResult<ContactSkillRepresentation>> Get(Guid id, bool isSkillsIncluded)
    {
      var entity = await _repository.GetById(id, isSkillsIncluded);
      if (entity == null) return NotFound();
      return _mapper.Map<ContactSkillRepresentation>(entity);
    }

    /// <summary>
    /// Update contact (from user)
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public virtual async Task<ActionResult<ContactSkillRepresentation>> Put(Guid id, ContactSkillSimplifiedRepresentation entity)
    {
      if (id != entity.Contact.Id) return BadRequest();
      var contact = _mapper.Map<ContactRepresentation, Contact>(entity.Contact);

      contact = await _repository.Update(contact, entity.SkillIds);
      return _mapper.Map<ContactSkillRepresentation>(contact);
    }

    /// <summary>
    /// Add contact
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<ContactSkillRepresentation>> Post(ContactSkillSimplifiedRepresentation entity)
    {
      var contact = _mapper.Map<ContactRepresentation, Contact>(entity.Contact);
      contact = await _repository.Create(contact, entity.SkillIds);
      return _mapper.Map<ContactSkillRepresentation>(contact);
    }

    /// <summary>
    /// Delete contact
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public virtual async Task<ActionResult<ContactRepresentation>> Delete(Guid id)
    {
      var entity = await _repository.Delete(id);
      if (entity == null) return NotFound();
      return _mapper.Map<ContactRepresentation>(entity);
    }
  }
}