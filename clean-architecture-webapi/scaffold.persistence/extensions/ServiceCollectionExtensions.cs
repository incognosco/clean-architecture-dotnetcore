using Scaffold.Application.Interfaces.Repositories;
using Scaffold.Application.Interfaces.Services;
using Scaffold.Domain.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Scaffold.Persistence.Contexts;
using Scaffold.Persistence.Repositories;
using Scaffold.Persistence.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Scaffold.Persistence.Extensions
{
    public static class ServiceCollectionExtensions
    {
       

        public static IServiceCollection AddGenericRepository(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
            return services;
        }

        public static IServiceCollection AddUnitOfWork(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork.UnitOfWork));
            services.AddTransient(typeof(IDapperDbContext), typeof(Persistence.Connections.DapperDbContext));
            services.AddTransient(typeof(IQueryDbContext), typeof(Persistence.Connections.QueryDbContext));
            return services;
        }

        public static void AddEmailService(this IServiceCollection services, IConfiguration _config)
        {
            services.Configure<MailSettings>(_config.GetSection("MailSettings"));
            services.AddTransient<IEmailService, EmailService>();
        }

        public static IServiceCollection AddTenantService(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped(typeof(ITenantService), typeof(TenantService));
            return services;
        }

        public static IServiceCollection AddMultiTenancy(this IServiceCollection services, IConfiguration config)
        {
            var options = services.GetOptions<TenantSettings>(nameof(TenantSettings));

            return services
                .AddDbContext<TenantDbContext>((p, m) =>
                {
                    // TODO: We should probably add specific dbprovider/connectionstring setting for the tenantDb with a fallback to the main databasesettings
                    var databaseSettings = options.Defaults;
                    m.UseOracle(databaseSettings.ConnectionString);
                }).AddScoped<ITenantService, TenantService>();
        }

        public static IServiceCollection AddCORS(this IServiceCollection services, 
            IConfiguration config)
        {
            var cors = services.GetOptions<CorsSettings>(nameof(CorsSettings));            
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy
                    .WithOrigins(
                        cors.CORS?.Split(","))
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
            return services;
        }

        public static IServiceCollection AddJwtAuthorization(this IServiceCollection services, IConfiguration config)
        {
            //Adding Athentication - JWT
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = config["JWTSettings:Issuer"],
                    ValidAudience = config["JWTSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWTSettings:Key"]))
                };
            });
            return services;
        }

        public static IServiceCollection AddTenantDatabases(this IServiceCollection services, IConfiguration config)
        {
            var options = services.GetOptions<TenantSettings>(nameof(TenantSettings));
            var defaultConnectionString = options.Defaults?.ConnectionString;
            services.AddDbContext<ApplicationDbContext>(
                options => options
                .UseOracle(defaultConnectionString), ServiceLifetime.Transient);

            //services.AddDbContext<newdbContext>(
            //    options =>
            //    options.UseNpgsql(defaultConnectionString));

            //services.AddDbContext<AppDbContext>(
            //   options =>
            //   options.UseLazyLoadingProxies()
            //   .UseNpgsql(defaultConnectionString));

            //if (defaultDbProvider.ToLower() == "postgresql")
            //{
            //    //services.AddDbContext<ApplicationDbContext>(m =>
            //    //{
            //    //    m.UseNpgsql(e => e.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            //    //});
            //    var tenants = options.Tenants;
            //    foreach (var tenant in tenants)
            //    {
            //        string connectionString;
            //        if (string.IsNullOrEmpty(tenant.ConnectionString))
            //        {
            //            connectionString = defaultConnectionString;
            //        }
            //        else
            //        {
            //            connectionString = tenant.ConnectionString;
            //        }
            //        //using var scope = services.BuildServiceProvider().CreateScope();
            //        //var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            //        //dbContext.Database.SetConnectionString(connectionString);
            //        //if (dbContext.Database.GetMigrations().Count() > 0)
            //        //{
            //        //    dbContext.Database.Migrate();
            //        //}
            //    }
            //}

            return services;
        }


        public static T GetOptions<T>(this IServiceCollection services, string sectionName) where T : new()
        {
            using var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var section = configuration.GetSection(sectionName);
            var options = new T();
            section.Bind(options);
            return options;
        }

    }
}
