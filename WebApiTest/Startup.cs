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
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddEFCore(Configuration);
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
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "API Demo", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        public static void AddFluentValidationExceptionHandler(this IServiceCollection services)
        {
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
        }

        public static void AddIdentityAuth(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", DomainParameter.Audience);
                });
            });

            services.AddAuthentication("Bear")
            .AddJwtBearer("Bear", options =>
            {
                options.Authority = DomainParameter.IdentityServerUrl;
                options.RequireHttpsMetadata = false;
                options.Audience = DomainParameter.Audience;
                //options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //ValidateIssuer = true,
                    ValidateAudience = true,
                    //ValidateLifetime = true,
                    //ValidateIssuerSigningKey = true,
                    //ValidIssuer = "http://localhost:61768/",
                    //ValidAudience = DomainParameter.WebApiTestUrl,
                    //TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ASEFRFDDWSDRGYHF")),
                    //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("veryVerySecretKey")),
                    //ClockSkew = TimeSpan.Zero
                };
            });
        }

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

        public static void AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection("TestConfigure");
            services.AddOptions().Configure<TestSetting>(section);
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
    }

    //static class ConfigureContainerExtention
    //{
    //    public static void RegisterCustomService(this ContainerBuilder builder, Assembly[] assemblies)
    //    {
    //        builder.RegisterAssemblyTypes(assemblies)
    //               .Where(t => t.GetCustomAttributes(typeof(ServiceAttribute), false).Length > 0
    //                            && t.GetCustomAttribute<ServiceAttribute>().Lifetime == ServiceLifetime.Singleton
    //                            && t.IsClass && !t.IsAbstract)
    //               .AsImplementedInterfaces()
    //               .SingleInstance();
    //        builder.RegisterAssemblyTypes(assemblies)
    //               .Where(t => t.GetCustomAttributes(typeof(ServiceAttribute), false).Length > 0
    //                            && t.GetCustomAttribute<ServiceAttribute>().Lifetime == ServiceLifetime.Transient
    //                            && t.IsClass && !t.IsAbstract)
    //               .AsImplementedInterfaces()
    //               .InstancePerDependency();
    //        builder.RegisterAssemblyTypes(assemblies)
    //               .Where(t => t.GetCustomAttributes(typeof(ServiceAttribute), false).Length > 0
    //                            && t.GetCustomAttribute<ServiceAttribute>().Lifetime == ServiceLifetime.Scoped
    //                            && t.IsClass && !t.IsAbstract)
    //               .AsImplementedInterfaces()
    //               .InstancePerLifetimeScope()
    //               .EnableInterfaceInterceptors().InterceptedBy(typeof(SomeInterceptor));
    //    }

    //    //builder.RegisterAssemblyTypes(typeof(Program).Assembly)
    //    //       .Where(x => x.Name.EndsWith("service", StringComparison.OrdinalIgnoreCase))
    //    //       .AsImplementedInterfaces()
    //    //       .InstancePerLifetimeScope()
    //    //       .EnableInterfaceInterceptors().InterceptedBy(typeof(AnyInterceptor));
    //    //builder.RegisterType(typeof(SomeInterceptor));
    //    //builder.RegisterType<ConnectionService>().As<IConnectionService>().SingleInstance();
    //    //builder.RegisterType<TestService>().As<ITestService>().InstancePerLifetimeScope();
    //    //builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
    //}

    //static class ConfigureExtention
    //{
    //    public static void UseSwaggerComponent(this IApplicationBuilder app)
    //    {
    //        app.UseSwagger();
    //        app.UseSwaggerUI(c =>
    //        {
    //            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Demo v1");
    //        });
    //    }
    //}

    //public class MappingProfile : Profile
    //{
    //    public MappingProfile()
    //    {
    //        CreateMap<Test1, Test2>().ReverseMap();
    //    }
    //}

    //public static class DomainParameter
    //{
    //    public static string IdentityServerUrl { get; set; } = "http://10.0.75.1:55009";
    //    public static string WebApiTestUrl { get; set; } = "http://10.0.75.1:55001";
    //    public static string Audience { get; set; } = "webapitest";
    //    public static string ContentA { get; set; }
    //    public static string ContentB { get; set; }
    //    public static string ContentC { get; set; }
    //    public static string ContentD { get; set; }
    //}
    #endregion
}
