using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using ContactManagement.WebApi.Extentions;

namespace LabTest.WebApi
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
            services.ResolveDependencies(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseHealthChecks("/health");
            loggerFactory.AddSerilog();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "labtest.api V1");
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert
                            .SerializeObject(new
                            {
                                messages = new[]
                                {
                                    "An unexpected fault happened. Try again later."
                                }
                            }));
                    });
                });

                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Error()
                    .WriteTo
                    .RollingFile(Path.Combine(env.WebRootPath + "/ExceptionLogs", "Exception-{Date}.txt"))
                    .CreateLogger();
            }

            app.Use(async (context, next) =>
            {
                await Task.Run(() => context.Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination"));
                await next();
            });

            app.UseCors("AllowAll");
            app.UseStaticFiles();
            //app.UseHttpsRedirection();
            app.UseMvc();

            app.ConfigAutoMapper();
        }
    }
}
