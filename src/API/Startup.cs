using System.IO;
using Dolores;
using Dolores.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TwoNil.API
{
   public class Startup
   {
      public readonly IConfigurationRoot Configuration;

      public Startup(IHostingEnvironment env)
      {
         var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("doloresSettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"doloresSettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
         Configuration = builder.Build();
      }

      // This method gets called by the runtime. Use this method to add services to the container.
      public void ConfigureServices(IServiceCollection services)
      {
         var doloresSettings = Configuration.GetSection("dolores").Get<DoloresSettings>();
         services.AddSingleton(doloresSettings);
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
      {
         //TODO Somehow console logging only logs level Information and higher???
         loggerFactory.AddConsole();

         // Debug logging logs everything to the output window in Visual Studio while debugging. Configuring the log level is not possible.
         loggerFactory.AddDebug();

         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }

         app.UseDoloresHttpHandlerMiddleware();
      }
   }
}
