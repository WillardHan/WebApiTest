using Autofac;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebApiTest.Filters;
using WebApiTest.Attributes;
using WebApiTest.Middlewares;
using WebApiTest.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using WebApiTest.Services;
using Autofac.Extras.DynamicProxy;
using WebApiTest.Interceptors;
//using AspectCore;
//using AspectCore.Extensions.DependencyInjection;
//using AspectCore.Configuration;

namespace WebApiTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // IHttpContextAccessor is required by the GetRequestService method.
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSwagger();
            services.AddControllers(
                options =>
            {
                options.Filters.Add<LoggerFilter>();
                options.Filters.Add<ExceptionFilter>();
            }
            ).AddControllersAsServices();
            //services.AddAspectConfigureServices();
            services.AddAutoMapper();
            services.AddCustomServices();
            services.AddOptions(Configuration);

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseSerilogRequestLogging();
            app.UseSwaggerComponent();
            app.UseHttpsRedirection();
            app.UseRouting();

            app.Use(async (context, next) =>
            {
                DomainParameter.ContentA = context.Request.Scheme;
                await next();
            });
            app.Map("/hello", MapRequest);
            app.UseMyCustomMiddleware();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var test = builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                   .Where(t => t.GetCustomAttributes(typeof(ServiceAttribute), false).Length > 0
                                && t.IsClass && !t.IsAbstract);


            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                   .Where(t => t.GetCustomAttributes(typeof(ServiceAttribute), false).Length > 0
                                && t.GetCustomAttribute<ServiceAttribute>().Lifetime == ServiceLifetime.Singleton
                                && t.IsClass && !t.IsAbstract)
                   .AsImplementedInterfaces()
                   .SingleInstance();

            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                   .Where(t => t.GetCustomAttributes(typeof(ServiceAttribute), false).Length > 0
                                && t.GetCustomAttribute<ServiceAttribute>().Lifetime == ServiceLifetime.Transient
                                && t.IsClass && !t.IsAbstract)
                   .AsImplementedInterfaces()
                   .InstancePerDependency();
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                   .Where(t => t.GetCustomAttributes(typeof(ServiceAttribute), false).Length > 0
                                && t.GetCustomAttribute<ServiceAttribute>().Lifetime == ServiceLifetime.Scoped
                                && t.IsClass && !t.IsAbstract)
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope()
                   .EnableInterfaceInterceptors().InterceptedBy(typeof(SomeInterceptor));

            //builder.RegisterAssemblyTypes(typeof(Program).Assembly)
            //       .Where(x => x.Name.EndsWith("service", StringComparison.OrdinalIgnoreCase))
            //       .AsImplementedInterfaces()
            //       .InstancePerLifetimeScope();
            //builder.RegisterType<ConnectionService>().As<IConnectionService>().SingleInstance();
            //builder.RegisterType<TestService>().As<ITestService>().InstancePerLifetimeScope();
            //builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
        }

        private static void MapRequest(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("This is MapRequest hello");
            });
        }
    }

    static class ConfigureServicesExtention
    {
        public static void AddAspectConfigureServices(this IServiceCollection services)
        {
            //services.ConfigureDynamicProxy(config =>
            //{
            //    config.Interceptors.AddTyped<CustomInterceptorAttribute>();
            //config.Interceptors.AddTyped<CustomInterceptorAttribute>(Predicates.ForNameSpace("WebApiTest.Services"));
            //config.Interceptors.AddTyped<CustomInterceptorAttribute>(args: new object[] { "custom" });
            //config.Interceptors.AddTyped<CustomInterceptorAttribute>(method => method.DeclaringType.Name.EndsWith("Service"));
            //config.Interceptors.AddTyped<CustomInterceptorAttribute>(Predicates.ForMethod("*Query")); //拦截所有Query后缀的方法
            //config.Interceptors.AddTyped<CustomInterceptorAttribute>(Predicates.ForService("*Repository")); //拦截所有Repository后缀的类或接口
            //config.Interceptors.AddTyped<CustomInterceptorAttribute>(Predicates.ForNameSpace("AspectCoreDemo.*")); //拦截所有AspectCoreDemo及其子命名空间下面的接口或类
            //config.NonAspectPredicates.AddNamespace("*.App1");//最后一级为App1的命名空间下的Service不会被代理
            //config.NonAspectPredicates.AddService("ICustomService");//ICustomService接口不会被代理
            //config.NonAspectPredicates.AddService("*Service");//后缀为Service的接口和类不会被代理
            //config.NonAspectPredicates.AddMethod("*Query");//后缀为Query的方法不会被代理
            //});
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Demo", Version = "v1" });
            });
        }

        public static void AddAutoMapper(this IServiceCollection services)
        {
            Mapper.Initialize(cfg => cfg.CreateMissingTypeMaps = true);

            //var mappingConfig = new MapperConfiguration(mc =>
            //{
            //    mc.AddProfile(new MappingProfile());
            //});
            //IMapper mapper = mappingConfig.CreateMapper();
            //services.AddSingleton(mapper);
        }

        public static void AddCustomServices(this IServiceCollection services)
        {
            //services.RegisterLifetimes(ServiceLifetime.Singleton);
            //services.RegisterLifetimes(ServiceLifetime.Scoped);
            //services.RegisterLifetimes(ServiceLifetime.Transient);

            //services.RegisterLifetimeTypes(typeof(ISingletonInterface));
            //services.RegisterLifetimeTypes(typeof(IScopedInterface));
            //services.RegisterLifetimeTypes(typeof(ITransientInterface));

            //services.AddScoped<IConnectionService, ConnectionService>();
            //services.AddScoped<ITestService, TestService>();
            //services.AddScoped<IUserService, UserService>();
        }

        private static void RegisterLifetimesByAttribute(this IServiceCollection services, ServiceLifetime serviceLifetime)
        {
            List<Type> types = AppDomain.CurrentDomain
                               .GetAssemblies()
                               .SelectMany(x => x.GetTypes())
                               .Where(t => t.GetCustomAttributes(typeof(ServiceAttribute), false).Length > 0
                                            && t.GetCustomAttribute<ServiceAttribute>().Lifetime == serviceLifetime
                                            && t.IsClass && !t.IsAbstract)
                               .ToList();
            foreach (var type in types)
            {
                Type[] interfaces = type.GetInterfaces();
                interfaces.ToList().ForEach(r =>
                {
                    services.AddSingleton(r, type);
                });
            }
        }

        private static void RegisterLifetimesByInhreit(this IServiceCollection services, Type lifetimeType)
        {
            List<Type> types = AppDomain.CurrentDomain
                               .GetAssemblies()
                               .SelectMany(x => x.GetTypes())
                               .Where(t => lifetimeType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                               .ToList();
            foreach (var type in types)
            {
                Type[] interfaces = type.GetInterfaces();
                interfaces.ToList().ForEach(r =>
                {
                    services.AddSingleton(r, type);
                });
            }
        }

        public static void AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection("TestConfigure");
            services.AddOptions().Configure<TestSetting>(section);
        }
    }

    static class ConfigureExtention
    {
        public static void UseSwaggerComponent(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Demo v1");
            });
        }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Test1, Test2>().ReverseMap();
        }
    }

    public static class DomainParameter
    {
        public static string ContentA { get; set; }
        public static string ContentB { get; set; }
        public static string ContentC { get; set; }
        public static string ContentD { get; set; }
    }
}
