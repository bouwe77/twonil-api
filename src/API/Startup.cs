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
      private readonly IConfigurationRoot _configuration;

      public Startup(IHostingEnvironment env)
      {
         var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("settings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"settings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
         _configuration = builder.Build();
      }

      // This method gets called by the runtime. Use this method to add services to the container.
      // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
      public void ConfigureServices(IServiceCollection services)
      {
         services.AddOptions();
         services.Configure<MyTEMPSettings>(_configuration.GetSection("dolores"));
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
      {
         // Note that console logging only logs level Information and higher. Somehow configuring the log level to Debug level does not seem to work...
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
