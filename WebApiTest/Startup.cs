using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApiTest.Application.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using WebApiTest.Domain;
using WebApiTest.Infrastructure.StartUp;
using Hangfire;
using WebApiTest.Application.Jobs;

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
            services.AddEFCore<DatabaseContext>(Configuration);
            services.AddSingleton<MongoDatabaseContext>();
            services.AddOptions<TestSetting>(Configuration, "TestConfigure");
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            base.Configure(app, env);
            RecurringJob.AddOrUpdate<RecordJob>(m => m.Execute(), Cron.Minutely);
        }
    }
}
