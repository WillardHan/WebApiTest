using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using WebApiTest.Infrastructure.StartUp;
using WebApiTest2.Domain;

namespace WebApiTest2
{
    public class Startup : BaseStartup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddEFCore(Configuration);
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            base.Configure(app, env);
        }
    }

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
            //    var section = configuration.GetSection("TestConfigure");
            //    services.AddOptions().Configure<TestSetting>(section);
        }
    }
}
