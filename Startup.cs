using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApiDeveloperChallenge.Common.Attributes;
using WebApiDeveloperChallenge.Common.Extensions;
using WebApiDeveloperChallenge.Models;
using WebApiDeveloperChallenge.Repositories;

namespace WebApiDeveloperChallenge
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();
      services.EnableSwaggerWithDefaultSettings();

      // Add DbContext
      services.AddDbContext<ContactsContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

      #region Add repositories

      services.AddScoped<ContactRepository>();
      services.AddScoped<SkillRepository>();
      services.AddScoped<UserRepository>();
      services.AddHttpContextAccessor();

      #endregion

      #region Add generic model validation

      services.AddScoped<ModelValidationAttribute>();

      #endregion

      services.AddSingleton(Configuration);
      services.EnableJWTTokenAuthentification(Configuration);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ContactsContext dataContext)
    {
      dataContext.Database.Migrate();

      if (env.IsDevelopment())
        app.UseDeveloperExceptionPage();

      app.UseSwagger();

      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "api version 1");
        c.RoutePrefix = string.Empty;
      });

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthorization();

      app.UseAuthentication();

      #region Add global error handling

      app.ConfigureCustomExceptionMiddleware();

      #endregion

      app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
  }
}