using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
    public readonly TRepository Repository;
    public readonly IMapper Mapper;

    protected BaseController(TRepository repository, IMapper mapper)
    {
      Repository = repository;
      Mapper = mapper;
    }

    /// <summary>
    /// Get list element
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public virtual async Task<ActionResult<IEnumerable<TEntityRepresentation>>> Get()
    {
      var entities = await Repository.GetAll();
      return entities.Select(e => Mapper.Map<TEntityRepresentation>(e)).ToList();
    }

    /// <summary>
    /// Get element
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
    /// Update element
    /// </summary>
    /// <param name="id"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public virtual async Task<ActionResult<TEntityRepresentation>> Put(Guid id, TEntityRepresentation entity)
    {
      if (id != entity.Id) return BadRequest();
      var baseEntity = Mapper.Map<TEntityRepresentation, TEntity>(entity);
      await Repository.Update(baseEntity);
      return AcceptedAtAction("Get", new {id = entity.Id}, entity);
    }

    /// <summary>
    /// Add new element
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    [HttpPost]
    public virtual async Task<ActionResult<TEntityRepresentation>> Post(TEntityRepresentation entity)
    {
      var baseEntity = Mapper.Map<TEntityRepresentation, TEntity>(entity);
      await Repository.Create(baseEntity);
      return Mapper.Map<TEntityRepresentation>(entity);
    }


    /// <summary>
    /// Delete element
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public virtual async Task<ActionResult<TEntityRepresentation>> Delete(Guid id)
    {
      var entity = await Repository.Delete(id);
      if (entity == null) return NotFound();
      return Mapper.Map<TEntityRepresentation>(entity);
    }
  }
}