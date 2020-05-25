using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiDeveloperChallenge.Common.Interfaces;

namespace WebApiDeveloperChallenge.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public abstract class BaseController<TEntity, TRepository> : ControllerBase
    where TEntity : class, IEntity
    where TRepository : IRepository<TEntity>
  {
    private readonly TRepository _repository;

    protected BaseController(TRepository repository)
    {
      _repository = repository;
    }

    [HttpGet]
    public virtual async Task<ActionResult<IEnumerable<TEntity>>> Get()
    {
      return await _repository.GetAll();
    }

    [HttpGet("{id}")]
    public virtual async Task<ActionResult<TEntity>> Get(Guid id)
    {
      var entity = await _repository.Get(id);
      if (entity == null) return NotFound();
      return entity;
    }

    [HttpPut("{id}")]
    public virtual async Task<ActionResult<TEntity>> Put(Guid id, TEntity entity)
    {
      if (id != entity.Id) return BadRequest();
      await _repository.Update(entity);
      return AcceptedAtAction("Get", new {id = entity.Id}, entity);
    }

    [HttpPost]
    public virtual async Task<ActionResult<TEntity>> Post(TEntity entity)
    {
      await _repository.Create(entity);
      return CreatedAtAction("Get", new {id = entity.Id}, entity);
    }

    [HttpDelete("{id}")]
    public virtual async Task<ActionResult<TEntity>> Delete(Guid id)
    {
      var entity = await _repository.Delete(id);
      if (entity == null) return NotFound();
      return entity;
    }
  }
}