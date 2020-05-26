using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApiDeveloperChallenge.Models;
using WebApiDeveloperChallenge.Repositories;
using WebApiDeveloperChallenge.Representations;

namespace WebApiDeveloperChallenge.Controllers
{
  [Route("api/[controller]")]
  public class UserController : BaseController<UserApplication, UserApplicationRepresentation, UserRepository>
  {
    private readonly IConfiguration _configuration;

    public UserController(UserRepository repository, IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(repository, mapper, httpContextAccessor)
    {
      _configuration = configuration;
    }

    #region Disable routes

    [NonAction]
    public override Task<ActionResult<IEnumerable<UserApplicationRepresentation>>> Get()
    {
      return base.Get();
    }

    [NonAction]
    public override Task<ActionResult<UserApplicationRepresentation>> Delete(Guid id)
    {
      return base.Delete(id);
    }

    [NonAction]
    public override Task<ActionResult<UserApplicationRepresentation>> Get(Guid id)
    {
      return base.Get(id);
    }

    [NonAction]
    public override Task<ActionResult<UserApplicationRepresentation>> Put(Guid id, UserApplicationRepresentation entity)
    {
      return base.Put(id, entity);
    }

    #endregion

    /// <summary>
    /// Get Token from user
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    [HttpGet("GetToken")]
    public async Task<ActionResult<string>> GetTokenAsync(string username, string password)
    {
      var userFounded = await Repository.GetUser(username, password);
      if (userFounded == null)
        return NotFound(username);

      var key = _configuration["JwtToken:SecretKey"];
      var issuer = _configuration["JwtToken:Issuer"];
      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

      //Create a List of Claims, Keep claims name short    
      var permClaims = new List<Claim>
      {
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, userFounded.Id.ToString()),
        new Claim(ClaimTypes.Name, userFounded.Username)
      };

      //Create Security Token object by giving required parameters    
      var token = new JwtSecurityToken(issuer,
        issuer,
        permClaims,
        expires: DateTime.UtcNow.AddHours(1),
        signingCredentials: credentials);
      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}