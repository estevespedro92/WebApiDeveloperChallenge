using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApiDeveloperChallenge.Models;
using WebApiDeveloperChallenge.Repositories;

namespace WebApiDeveloperChallenge.Controllers
{
  [Route("api/[controller]")]
  public class UserController : BaseController<UserApplication, UserRepository>
  {
    private readonly IConfiguration _configuration;
    private readonly UserRepository _repository;

    public UserController(UserRepository repository, IConfiguration configuration) : base(repository)
    {
      _repository = repository;
      _configuration = configuration;
    }

    #region Disable routes

    [NonAction]
    public override Task<ActionResult<IEnumerable<UserApplication>>> Get()
    {
      return base.Get();
    }

    [NonAction]
    public override Task<ActionResult<UserApplication>> Delete(Guid id)
    {
      return base.Delete(id);
    }

    [NonAction]
    public override Task<ActionResult<UserApplication>> Get(Guid id)
    {
      return base.Get(id);
    }

    [NonAction]
    public override Task<ActionResult<UserApplication>> Put(Guid id, UserApplication entity)
    {
      return base.Put(id, entity);
    }
    #endregion

    [HttpGet("GetToken")]
    public async Task<ActionResult<string>> GetTokenAsync(string username, string password)
    {
      var userFounded = await _repository.GetUser(username, password);
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