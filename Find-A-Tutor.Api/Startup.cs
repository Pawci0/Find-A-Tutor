﻿using Find_A_Tutor.Api.Framework;
using Find_A_Tutor.Core.Repositories;
using Find_A_Tutor.Infrastructure.EF;
using Find_A_Tutor.Core.Mappers;
using Find_A_Tutor.Infrastructure.Repositories;
using Find_A_Tutor.Core.Services;
using Find_A_Tutor.Core.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NLog.Extensions.Logging;
using NLog.Web;
using System.Text;
using AutoMapper;
using System.Reflection;

namespace Find_A_Tutor.Api
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddJsonOptions(x => x.SerializerSettings.Formatting = Formatting.Indented);

            services.AddAuthorization(x => x.AddPolicy("HasAdminRole", p => p.RequireRole("admin")));
            services.AddAuthorization(x => x.AddPolicy("HasTutorRole", p => p.RequireRole("tutor")));
            services.AddAuthorization(x => x.AddPolicy("HasStudentRole", p => p.RequireRole("student")));

            services.AddScoped<IPrivateLessonService, PrivateLessonService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISchoolSubjectService, SchoolSubjectService>();

            services.Configure<SqlSettings>(Configuration);
            var sqlSettings = Configuration.GetSection("sql").Get<SqlSettings>();
            services.AddSingleton<SqlSettings>(sqlSettings); //todo: Options pattern

            if (sqlSettings.InMemory)
            {
                services.AddScoped<IPrivateLessonRepository, InMemoryPrivateLessonRepository>();
                services.AddScoped<IUserRepository, InMemoryUserRepository>();
                services.AddScoped<ISchoolSubjectRepository, InMemorySchoolSubjectRepository>();
            }
            else
            {
                services.AddScoped<IPrivateLessonRepository, PrivateLessonRepository>();
                services.AddScoped<IUserRepository, UserRepository>();
                services.AddScoped<ISchoolSubjectRepository, SchoolSubjectRepository>();
            }            

            services.AddEntityFrameworkSqlServer()
                    .AddDbContext<FindATurorContext>(options => options.UseSqlServer(sqlSettings.ConnectionString));

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            //todo: DataInitializer
            //services.AddScoped<IDataInitializer, DataInitializer>(); 

            services.AddSingleton<IJwtHandler, JwtHandler>();

            //var appSettings = Configuration.GetSection("app");
            //services.Configure<AppSettings>(appSettings);

            var jwtSettings = Configuration.GetSection("jwt");
            services.Configure<JwtSettings>(jwtSettings);

            services.AddAuthentication().AddJwtBearer(o =>
            {
                o.IncludeErrorDetails = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtSettings.GetValue<string>("issuer"),
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetValue<string>("key")))
                };
            });

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();
            env.ConfigureNLog("nlog.config");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();                
            }

            if (env.IsProduction() || env.IsStaging())
            {
                //todo: env viarables working
            }

            app.UseErrorHandler();

            app.UseAuthentication();
            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
