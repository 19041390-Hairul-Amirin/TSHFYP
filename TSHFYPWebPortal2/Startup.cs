using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using AppOwnsData.Models;
using AppOwnsData.Services;
using TSHFYPWebPortal2.Models;
using Microsoft.EntityFrameworkCore;

namespace TSHFYPWebPortal2
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
            services.AddControllersWithViews();


            services.AddScoped(typeof(AadService))
                   .AddScoped(typeof(PbiEmbedService));
            // Loading appsettings.json in C# Model classes
            services.Configure<AzureAd>(Configuration.GetSection("AzureAd"))
                    .Configure<PowerBI>(Configuration.GetSection("PowerBI"));


            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options => {
                        options.LoginPath = "/Account/Login/";
                        options.AccessDeniedPath = "/Account/Forbidden/";
                    });


            //Getting Connection String from appsettings.json
            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connection));
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
        }
    }
}
