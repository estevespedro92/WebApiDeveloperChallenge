using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace WebApiDeveloperChallenge.Common.Extensions
{
  public static class ServiceCollectionExtension
  {
    /// <summary>
    /// </summary>
    /// <param name="services"></param>
    public static void EnableSwaggerWithDefaultSettings(this IServiceCollection services)
    {
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo {Title = "Default API", Version = "v1"});
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
          Name = "Authorization",
          Scheme = "Bearer",
          In = ParameterLocation.Header,
          Description = "JWT Authorization header using the Bearer scheme.",
          Type = SecuritySchemeType.ApiKey
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
          {
            new OpenApiSecurityScheme
            {
              Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header
            },
            new string[] { }
          }
        });
      });
    }

    /// <summary>
    ///   Enable JWT token authentification
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void EnableJWTTokenAuthentification(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddAuthentication(option =>
      {
        option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(options =>
      {
        options.SaveToken = true;
        options.RequireHttpsMetadata = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = false,
          ValidateIssuerSigningKey = true,
          ValidIssuer = configuration["JwtToken:Issuer"],
          ValidAudience = configuration["JwtToken:Issuer"],
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtToken:SecretKey"]))
        };
      });
    }
  }
}