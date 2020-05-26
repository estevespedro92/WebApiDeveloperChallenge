using System;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApiDeveloperChallenge.Common.Classes;
using WebApiDeveloperChallenge.Common.Globals;

namespace WebApiDeveloperChallenge.Common.Extensions
{
  public static class ServiceCollectionExtension
  {
    /// <summary>
    ///   Enable swagger with default settings
    /// </summary>
    /// <param name="services"></param>
    public static void EnableSwaggerWithDefaultSettings(this IServiceCollection services)
    {
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
          Title = "Default API", 
          Version = "v1",
          Description = "API for WebApiDeveloper Challenge",
          Contact = new OpenApiContact
          {
            Email = "estevespereirapedro@icloud.com",
            Name = "Pedro Esteves",
          }
        });
        c.AddSecurityDefinition(GlobalApplicationSettings.BEARER_NAME, new OpenApiSecurityScheme
        {
          Name = "Authorization",
          Scheme = GlobalApplicationSettings.BEARER_NAME,
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
                Id = GlobalApplicationSettings.BEARER_NAME
              },
              Scheme = "oauth2",
              Name = GlobalApplicationSettings.BEARER_NAME,
              In = ParameterLocation.Header
            },
            new string[] { }
          }
        });

        // Include xml documentation 
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
      });
    }

    /// <summary>
    ///   Enable JWT token authentification
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void EnableJwtTokenAuthentification(this IServiceCollection services, IConfiguration configuration)
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
          ValidIssuer = configuration[GlobalApplicationSettings.JWT_TOKEN_ISSUER_NAME],
          ValidAudience = configuration[GlobalApplicationSettings.JWT_TOKEN_ISSUER_NAME],
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration[GlobalApplicationSettings.JWT_TOKEN_SECRET_KEY_NAME]))
        };
      });
    }
  }
}