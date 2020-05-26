using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiDeveloperChallenge.Models;
using WebApiDeveloperChallenge.Repositories;
using WebApiDeveloperChallenge.Representations;

namespace WebApiDeveloperChallenge.Controllers
{

  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  [Route("api/[controller]")]
  [ApiController]
  public class SkillsController : BaseController<Skill, SkillRepresentation, SkillRepository>
  {
    public SkillsController(SkillRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(repository, mapper, httpContextAccessor)
    {
    }
  }
}