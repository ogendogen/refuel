using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Utils;

namespace Refuel
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.secrets.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<RefuelContext>();
            services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizeFolder("/Panel");
            });

            services.AddScoped<IUsersManager, UsersManager>();
            services.AddScoped<IEmailManager, EmailManager>();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddMvc();
            services.AddOptions();
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.Configure<Passwords>(Configuration.GetSection("Passwords"));
            services.Configure<Recaptcha>(Configuration.GetSection("Recaptcha"));
            services.Configure<GoogleAuth>(Configuration.GetSection("GoogleAuth"));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie()
                .AddGoogle(options =>
                {
                    Passwords passwords = Configuration.GetSection("Passwords").Get<Passwords>();
                    GoogleAuth googleAuth = Configuration.GetSection("GoogleAuth").Get<GoogleAuth>();

                    options.ClientId = googleAuth.SiteKey;
                    options.ClientSecret = passwords.GoogleSecretKey;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCookiePolicy(new CookiePolicyOptions()
            {
                MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Strict
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
