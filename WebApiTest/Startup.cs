using Autofac;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebApiTest.Application.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using WebApi.Infrastructure.LifetimeInterfaces;
using WebApiTest.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApi.Infrastructure.Attributes;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApiTest.Infrastructure.StartUp;

namespace WebApiTest
{
    public class Startup : BaseStartup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddEFCore(Configuration);
            services.AddSingleton<MongoDatabaseContext>();
            services.AddOptions(Configuration);
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            base.Configure(app, env);
        }
    }

    #region Extensions
    static class ConfigureServicesExtention
    {
        public static void AddEFCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(configuration.GetValue<string>("ConnectionString"), options =>
                {
                    options.CommandTimeout(60);
                })
                .LogTo(Console.WriteLine, LogLevel.Information);
            });
        }

        public static void AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection("TestConfigure");
            services.AddOptions().Configure<TestSetting>(section);
        }
    }
    #endregion
}
