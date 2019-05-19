using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using ContactManagement.Core.Data;
using ContactManagement.Core.Repositories.Abstractions;
using ContactManagement.Core.Repositories.Implementations;
using DaffodilSoftware.Core.SharedKernel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ContactManagement.WebApi.Extentions
{
    public static class DependencyInversions
    {
        public static void ResolveDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHealthChecks()
                      .AddDbContextCheck<ContactManagementContext>();
            services.AddSingleton(configuration);
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddScoped<IUserInfoRepository, UserInfoRepository>();

            services.AddScoped<IContactInfoRepository, ContactInfoRepository>();
            services.AddScoped<IContactCategoryRepository, ContactCategoryRepository>();

            services
                  .AddDbContext<ContactManagementContext>(option =>
                  {
                      option.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                      option.UseSqlServer(configuration.GetConnectionString("ContactManagementContext"));

                  });

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Info
                {
                    Title = "Lab Test Web API",
                    Version = "1.0",
                    Contact = new Contact
                    {
                        Name = "Arif"
                    },
                    Description = "Contact Management Web API",
                    TermsOfService = "Terms Of Service"
                });
            });

            services.Configure<ApiBehaviorOptions>(o => { o.SuppressModelStateInvalidFilter = true; });
           
            services
                .AddMvc(options =>
                {
                    options.Filters.Add(typeof(ValidateModelStateFilter));
                });
            //    .AddFluentValidation(v => v.RegisterValidatorsFromAssemblyContaining<SyllabusCreatingValidator>());

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(implementationFactory =>
            {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>()
                    .ActionContext;
                return new UrlHelper(actionContext);
            });

            var secret = configuration["AppSecret"].ToString();
            var key = Convert.FromBase64String(secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }
    }
}
