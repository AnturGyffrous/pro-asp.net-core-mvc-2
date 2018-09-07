﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Users.Infrastructure;
using Users.Models;

namespace Users
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IPasswordValidator<AppUser>, CustomPasswordValidator>();
            services.AddTransient<IUserValidator<AppUser>, CustomUserValidator>();
            services.AddTransient<IClaimsTransformation, LocationClaimsProvider>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("DCUsers", policy =>
                {
                    policy.RequireRole("Users");
                    policy.RequireClaim(ClaimTypes.StateOrProvince, "DC");
                });
            });

            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(Configuration["Data:SportStoreIdentity:ConnectionString"]));

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                //options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz";
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            })
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/User/Login";
                options.AccessDeniedPath = "/User/AccessDenied";
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStatusCodePages();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
            //AppIdentityDbContext.CreateAdminAccount(app.ApplicationServices, Configuration).Wait();
        }
    }
}