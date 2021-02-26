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
using WebApiTest.Application.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Autofac.Extras.DynamicProxy;
using WebApi.Infrastructure.Interceptors;
using Castle.DynamicProxy;
using WebApi.Infrastructure.LifetimeInterfaces;
using WebApiTest.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApi.Infrastructure.Filters;
using WebApi.Infrastructure.Middlewares;
using WebApi.Infrastructure.Attributes;
using MediatR.Extensions.Autofac.DependencyInjection;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WebApiTest.Infrastructure.Repository;
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
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSwagger();
            services.AddControllers(options =>
            {
                options.Filters.Add<LoggerFilter>();
                options.Filters.Add<ExceptionFilter>();
            })
            .AddControllersAsServices()
            .SetCompatibilityVersion(CompatibilityVersion.Latest)
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                //options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            })
            .AddFluentValidation(config =>
            {
                config.ImplicitlyValidateChildProperties = true;
            });
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errorMessage = actionContext.ModelState
                                .Where(e => e.Value.Errors.Count > 0)
                                .Select(e => e.Value.Errors.First().ErrorMessage)
                                .FirstOrDefault();
                    return new BadRequestObjectResult(errorMessage);
                };
            });
            services.AddAutoMapper();
            services.AddDbContext<DatabaseContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(Configuration.GetValue<string>("ConnectionString"), options =>
                {
                    options.CommandTimeout(60);
                })
                .LogTo(Console.WriteLine, LogLevel.Information);
            });
            //services.AddCustomServices();
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
            var refAssembyNames = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
            foreach (var asslembyNames in refAssembyNames)
            {
                Assembly.Load(asslembyNames);
            }
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            builder.RegisterMediatR(assemblies);
            builder.RegisterAssemblyTypes(assemblies).Where(t => typeof(IValidator).IsAssignableFrom(t)).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(assemblies).Where(t => typeof(IInterceptor).IsAssignableFrom(t));
            builder.RegisterAssemblyTypes(assemblies).AsClosedTypesOf(typeof(IRepository<>));
            builder.RegisterCustomService(assemblies);
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

        public static void AddAspectConfigureServices(this IServiceCollection services)
        {
            //services.ConfigureDynamicProxy(config =>
            //{
            //    config.Interceptors.AddTyped<CustomInterceptorAttribute>();
            //config.Interceptors.AddTyped<CustomInterceptorAttribute>(Predicates.ForNameSpace("WebApiTest.Services"));
            //config.Interceptors.AddTyped<CustomInterceptorAttribute>(args: new object[] { "custom" });
            //config.Interceptors.AddTyped<CustomInterceptorAttribute>(method => method.DeclaringType.Name.EndsWith("Service"));
            //config.Interceptors.AddTyped<CustomInterceptorAttribute>(Predicates.ForMethod("*Query")); 
            //config.Interceptors.AddTyped<CustomInterceptorAttribute>(Predicates.ForService("*Repository")); 
            //config.Interceptors.AddTyped<CustomInterceptorAttribute>(Predicates.ForNameSpace("AspectCoreDemo.*")); 
            //config.NonAspectPredicates.AddNamespace("*.App1");
            //config.NonAspectPredicates.AddService("ICustomService");
            //config.NonAspectPredicates.AddService("*Service");
            //config.NonAspectPredicates.AddMethod("*Query");
            //});
        }

        public static void AddCustomServices(this IServiceCollection services)
        {
            services.RegisterLifetimesByAttribute(ServiceLifetime.Singleton);
            services.RegisterLifetimesByAttribute(ServiceLifetime.Scoped);
            services.RegisterLifetimesByAttribute(ServiceLifetime.Transient);

            //services.RegisterLifetimesByInhreit(typeof(ISingletonInterface));
            //services.RegisterLifetimesByInhreit(typeof(IScopedInterface));
            //services.RegisterLifetimesByInhreit(typeof(ITransientInterface));
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
                    switch (serviceLifetime)
                    {
                        case ServiceLifetime.Singleton: services.AddSingleton(r, type); break;
                        case ServiceLifetime.Scoped: services.AddScoped(r, type); break;
                        case ServiceLifetime.Transient: services.AddTransient(r, type); break;
                    }
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
                    if (lifetimeType == typeof(ISingletonInterface)) services.AddSingleton(r, type);
                    else if (lifetimeType == typeof(IScopedInterface)) services.AddScoped(r, type);
                    else if (lifetimeType == typeof(ITransientInterface)) services.AddTransient(r, type);
                });
            }
        }

        public static void AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection("TestConfigure");
            services.AddOptions().Configure<TestSetting>(section);
        }
    }

    static class ConfigureContainerExtention
    {
        public static void RegisterCustomService(this ContainerBuilder builder, Assembly[] assemblies)
        {
            builder.RegisterAssemblyTypes(assemblies)
                   .Where(t => t.GetCustomAttributes(typeof(ServiceAttribute), false).Length > 0
                                && t.GetCustomAttribute<ServiceAttribute>().Lifetime == ServiceLifetime.Singleton
                                && t.IsClass && !t.IsAbstract)
                   .AsImplementedInterfaces()
                   .SingleInstance();
            builder.RegisterAssemblyTypes(assemblies)
                   .Where(t => t.GetCustomAttributes(typeof(ServiceAttribute), false).Length > 0
                                && t.GetCustomAttribute<ServiceAttribute>().Lifetime == ServiceLifetime.Transient
                                && t.IsClass && !t.IsAbstract)
                   .AsImplementedInterfaces()
                   .InstancePerDependency();
            builder.RegisterAssemblyTypes(assemblies)
                   .Where(t => t.GetCustomAttributes(typeof(ServiceAttribute), false).Length > 0
                                && t.GetCustomAttribute<ServiceAttribute>().Lifetime == ServiceLifetime.Scoped
                                && t.IsClass && !t.IsAbstract)
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope()
                   .EnableInterfaceInterceptors().InterceptedBy(typeof(SomeInterceptor));
        }

        //builder.RegisterAssemblyTypes(typeof(Program).Assembly)
        //       .Where(x => x.Name.EndsWith("service", StringComparison.OrdinalIgnoreCase))
        //       .AsImplementedInterfaces()
        //       .InstancePerLifetimeScope()
        //       .EnableInterfaceInterceptors().InterceptedBy(typeof(AnyInterceptor));
        //builder.RegisterType(typeof(SomeInterceptor));
        //builder.RegisterType<ConnectionService>().As<IConnectionService>().SingleInstance();
        //builder.RegisterType<TestService>().As<ITestService>().InstancePerLifetimeScope();
        //builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
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
