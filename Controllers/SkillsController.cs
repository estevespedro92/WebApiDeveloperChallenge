using Microsoft.AspNetCore.Mvc;
using WebApiDeveloperChallenge.Models;
using WebApiDeveloperChallenge.Repositories;

namespace WebApiDeveloperChallenge.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SkillsController : BaseController<Skill, SkillRepository>
  {
    public SkillsController(SkillRepository repository) : base(repository)
    {
    }
  }
}