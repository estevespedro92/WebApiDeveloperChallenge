using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiDeveloperChallenge.Common.Exceptions;
using WebApiDeveloperChallenge.Common.Extensions;
using WebApiDeveloperChallenge.Common.Interfaces;
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
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ContactRepository _repository;
    private readonly SkillRepository _skillRepository;

    public ContactsController(ContactRepository repository, SkillRepository skillRepository, IMapper mapper,
      IHttpContextAccessor httpContextAccessor)
    {
      _repository = repository;
      _skillRepository = skillRepository;
      _mapper = mapper;
      _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    ///   Get lists from contact (from user)
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
    ///   Get contact (from user)
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
    ///   Update contact (from user)
    /// </summary>
    /// <param name="id"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public virtual async Task<ActionResult<ContactSkillRepresentation>> Put(Guid id,
      ContactSkillSimplifiedRepresentation entity)
    {
      if (!id.Equals(entity.Contact.Id)) return BadRequest();
      var contact = _mapper.Map<ContactRepresentation, Contact>(entity.Contact);

      var result = await CheckUserIdPermission(id, entity.SkillIds.ToList());
      if (result is NotFoundResult)
        return result;

      contact = await _repository.Update(contact, entity.SkillIds);
      return _mapper.Map<ContactSkillRepresentation>(contact);
    }

    /// <summary>
    ///   Add contact
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
    ///   Delete contact
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ContactRepresentation>> Delete(Guid id)
    {
      await CheckUserIdPermission(id);
      var entity = await _repository.Delete(id);
      if (entity == null) return NotFound();
      return _mapper.Map<ContactRepresentation>(entity);
    }

    /// <summary>
    ///   Check if changes are made by data user
    /// </summary>
    /// <param name="id"></param>
    /// <param name="skillIds"></param>
    /// <returns></returns>
    private async Task<ActionResult> CheckUserIdPermission(Guid id, List<Guid> skillIds = null)
    {
      // Check if current user is the same from entity
      var oldEntity = (IUserIdEntity) await _repository.GetWhitoutUserId(id);
      if (oldEntity == null)
        return NotFound();

      var currentId = _httpContextAccessor.HttpContext.User.GetUserId();

      if (!oldEntity.UserId.Equals(currentId))
        throw new UserIdRelatedDataException();

      if (skillIds == null)
        return Ok();

      if (!skillIds.Any())
        return Ok();

      var skills = await _skillRepository.GetAllWithoutUserId();

      var currentSkills = skills.Where(s => skillIds.Contains(s.Id)).ToList();
      if (!currentSkills.Any())
        return Ok();

      if (currentSkills.Any(s => !s.UserId.Equals(currentId)))
        throw new UserIdRelatedDataUsedException();

      return Ok();
    }
  }
}