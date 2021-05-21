using Autofac;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Autofac.Extras.DynamicProxy;
using WebApi.Infrastructure.Interceptors;
using Castle.DynamicProxy;
using WebApi.Infrastructure.LifetimeInterfaces;
using WebApi.Infrastructure.Filters;
using WebApi.Infrastructure.Middlewares;
using WebApi.Infrastructure.Attributes;
using MediatR.Extensions.Autofac.DependencyInjection;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure.HttpClients;
using WebApi.Infrastructure.Repository;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using Newtonsoft.Json;
using Serilog;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using System.IO;
using WebApi.Infrastructure.Hangfire;
using WebApi.Infrastructure.Cap;
//using AspectCore;
//using AspectCore.Extensions.DependencyInjection;
//using AspectCore.Configuration;

namespace WebApiTest.Infrastructure.StartUp
{
    public class BaseStartup
    {
        public IConfiguration Configuration { get; set; }
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSwagger();
            services.AddHangFireSerivce(Configuration);
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
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            })
            .AddFluentValidation(config =>
            {
                config.ImplicitlyValidateChildProperties = true;
            });
            services.AddFluentValidationExceptionHandler();
            services.AddIdentityAuth(Configuration);
            services.AddAutoMapper();
            services.AddRedisService(Configuration);
            services.AddTransient<HttpClientTokenHandler>();
            services.AddHttpClientWithToken();
            services.AddCapService();
            services.AddCapService(Configuration);
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseSerilogRequestLogging();
            //app.UseHttpsRedirection();
            app.UseSwaggerComponent();
            app.UseHangfireDashboard("/hangfire", new Hangfire.DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            #region test middleware
            app.Use(async (context, next) =>
            {
                await next();
            });
            app.Map("/hello", MapRequest);
            app.UseMyCustomMiddleware();
            #endregion

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers().RequireAuthorization("ApiScope");
                endpoints.MapControllers();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                //endpoints.MapHangfireDashboard();
            });
        }

        public virtual void ConfigureContainer(ContainerBuilder builder)
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

    public static class ConfigureServicesExtentionForAll
    {
        public static void AddEFCore<T>(this IServiceCollection services, IConfiguration configuration) where T : DbContext
        {
            services.AddDbContext<T>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(configuration.GetValue<string>("ConnectionString"), options =>
                {
                    options.CommandTimeout(60);
                })
                .LogTo(Console.WriteLine, LogLevel.Information);
            });
        }

        public static void AddOptions<T>(this IServiceCollection services, IConfiguration configuration, string NodeName) where T : class 
        {
            var section = configuration.GetSection("TestConfigure");
            services.AddOptions().Configure<T>(section);
        }
    }

    static class ConfigureServicesExtention
    {
        public static void AddHangFireSerivce(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetValue<string>("ConnectionString"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            services.AddHangfireServer();
        }

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

        public static void AddIdentityAuth(this IServiceCollection services, IConfiguration configuration)
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
                options.Authority = configuration.GetValue<string>("Identityserver_URL");
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

        public static void AddCapService(this IServiceCollection services)
        {
            List<Type> types = AppDomain.CurrentDomain
                               .GetAssemblies()
                               .SelectMany(x => x.GetTypes())
                               .Where(t => typeof(ICapSubscribe).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                               .ToList();
            foreach (var type in types)
            {
                Type[] interfaces = type.GetInterfaces();
                interfaces.ToList().ForEach(r =>
                {
                    services.AddTransient(r, type);
                });
            }
        }

        public static void AddRedisService(this IServiceCollection services, IConfiguration configuration)
        {
            var connstr = configuration.GetValue<string>("Redis_ConnectionString");
            var options = ConfigurationOptions.Parse(connstr, true);
            options.ConnectTimeout = 10000;
            options.SyncTimeout = 10000;
            options.ConnectRetry = 8;
            options.ResolveDns = true;
            options.AbortOnConnectFail = true;
            services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(options));
        }

        public static void AddMongoService(this IServiceCollection services, IConfiguration configuration)
        {

        }

        public static void AddHttpClientWithToken(this IServiceCollection services)
        {
            services.AddHttpClient("token")
            .AddHttpMessageHandler<HttpClientTokenHandler>()
            .ConfigureHttpClient((sp, httpClient) =>
            {
                //httpClient.BaseAddress = new Uri(Configuration.GetValue<string>("Base_URL"));
            });
        }

        public static void AddCapService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCap(x =>
            {
                x.UseSqlServer(configuration.GetValue<string>("ConnectionString"));
                x.UseRabbitMQ(m => {
                    m.HostName = configuration.GetValue<string>("RabbitMq_HostName");
                    m.UserName = configuration.GetValue<string>("RabbitMq_UserName");
                    m.Password = configuration.GetValue<string>("RabbitMq_Password");
                });

                x.SucceedMessageExpiredAfter = 24 * 3600;
                x.FailedRetryCount = 5;

                x.UseDashboard(opt =>
                {
                    opt.PathMatch = "/cap";
                    opt.Authorization = new[] { new CapAuthorizationFilter() };
                });

                //x.UseDiscovery(d =>
                //{
                //    d.DiscoveryServerHostName = "localhost";
                //    d.DiscoveryServerPort = 8500;
                //    d.CurrentNodeHostName = "localhost";
                //    d.CurrentNodePort = 5800;
                //    d.NodeId = "1";
                //    d.NodeName = "CAP No.1 Node";
                //});
            });
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

    public static class DomainParameter
    {
        public static string Audience { get; set; } = "webapitest";
    }
}
