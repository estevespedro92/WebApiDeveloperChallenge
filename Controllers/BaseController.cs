using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiDeveloperChallenge.Common;
using WebApiDeveloperChallenge.Common.Exceptions;
using WebApiDeveloperChallenge.Common.Extensions;
using WebApiDeveloperChallenge.Common.Interfaces;

namespace WebApiDeveloperChallenge.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public abstract class BaseController<TEntity, TEntityRepresentation, TRepository> : ControllerBase
    where TEntity : class, IEntity
    where TEntityRepresentation : class, IEntity
    where TRepository : IRepository<TEntity>
  {
    public readonly IHttpContextAccessor HttpContextAccessor;
    public readonly IMapper Mapper;
    public readonly TRepository Repository;

    protected BaseController(TRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
      Repository = repository;
      Mapper = mapper;
      HttpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    ///   Get list element
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public virtual async Task<ActionResult<IEnumerable<TEntityRepresentation>>> Get()
    {
      var entities = await Repository.GetAll();
      return entities.Select(e => Mapper.Map<TEntityRepresentation>(e)).ToList();
    }

    /// <summary>
    ///   Get element
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public virtual async Task<ActionResult<TEntityRepresentation>> Get(Guid id)
    {
      var entity = await Repository.Get(id);
      if (entity == null) return NotFound();
      return Mapper.Map<TEntityRepresentation>(entity);
    }

    /// <summary>
    ///   Update element
    /// </summary>
    /// <param name="id"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public virtual async Task<ActionResult<TEntityRepresentation>> Put(Guid id, TEntityRepresentation entity)
    {
      if (!id.Equals(entity.Id)) return BadRequest();
      var baseEntity = Mapper.Map<TEntityRepresentation, TEntity>(entity);

      // Check if current user
      if (baseEntity is IUserIdEntity)
      {
        var result = await CheckUserIdPermission(id);
        if (result is NotFoundResult)
          return result;
      }


      await Repository.Update(baseEntity);
      return AcceptedAtAction("Get", new {id = entity.Id}, entity);
    }

    /// <summary>
    ///   Add new element
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    [HttpPost]
    public virtual async Task<ActionResult<TEntityRepresentation>> Post(TEntityRepresentation entity)
    {
      var baseEntity = Mapper.Map<TEntityRepresentation, TEntity>(entity);
      baseEntity = await Repository.Create(baseEntity);
      return Mapper.Map<TEntityRepresentation>(baseEntity);
    }

    /// <summary>
    ///   Delete element
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public virtual async Task<ActionResult<TEntityRepresentation>> Delete(Guid id)
    {
      var oldEntity = await ((IUserRepository<TEntity>) Repository).GetWithoutUserId(id);

      if (oldEntity is IUserIdEntity)
        await CheckUserIdPermission(id);

      var entity = await Repository.Delete(id);
      if (entity == null) return NotFound();
      return Mapper.Map<TEntityRepresentation>(entity);
    }

    /// <summary>
    ///   Check if changes are made by data user
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private async Task<ActionResult> CheckUserIdPermission(Guid id)
    {
      // Check if current user
      var oldEntity = (IUserIdEntity) await ((IUserRepository<TEntity>) Repository).GetWithoutUserId(id);
      if (oldEntity == null)
        return NotFound();

      if (!oldEntity.UserId.Equals(HttpContextAccessor.HttpContext.User.GetUserId()))
        throw new UserIdRelatedDataException();

      return Ok();
    }
  }
}