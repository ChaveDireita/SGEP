using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGEP.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using SGEP.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using SGEP.Areas.Identity.Services;

namespace SGEP
{
    public class Startup
    {
        public Startup (IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices (IServiceCollection services)
        {

            services.Configure<CookiePolicyOptions> (options =>
             {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                 options.MinimumSameSitePolicy = SameSiteMode.None;
             });

            services.AddDbContext<ApplicationDbContext> (options =>
            {
                options.UseLazyLoadingProxies(true);
                // options.UseMySql (Configuration.GetConnectionString ("mysql"));
                options.UseSqlServer (Configuration.GetConnectionString ("local"));
            });

            services.AddScoped<IEmailSender, EmailSender>();

            services.AddIdentity<SGEPUser, IdentityRole> (o => 
            {
                o.SignIn.RequireConfirmedEmail = false;
                o.SignIn.RequireConfirmedPhoneNumber = false;
                o.User.RequireUniqueEmail = true;
                o.Password.RequireDigit = false;
                o.Password.RequiredLength = 3;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireLowercase = false;
            }).AddDefaultTokenProviders()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddEntityFrameworkStores<ApplicationDbContext> ();

            services.AddMvc (config => 
            {
                config.Filters.Add (new AuthorizeFilter (
                    new AuthorizationPolicyBuilder ().RequireAuthenticatedUser ()
                                                     .Build ()));
            }).SetCompatibilityVersion (CompatibilityVersion.Version_2_1);
            
            services.AddScoped<RolesSeeder>();

            services.ConfigureApplicationCookie(o => 
            {
                o.LoginPath = "/Identity/Account/Login";
                o.AccessDeniedPath = "/Identity/Account/AccessDenied";
                o.LogoutPath="/Identity/Account/Logout";
            });
            string email = Configuration["EmailService:email"];
            string pass = Configuration["EmailService:password"];
            string smtpAddr = Configuration["EmailService:smtp-address"];
            int smtpPort = int.Parse(Configuration["EmailService:smtp-port"]);
            EmailSender.Configuration = new EmailServiceConfiguration { Email = email, Password = pass, SmtpAddress = smtpAddr, SmtpPort = smtpPort};
            // services.Configure<IISOptions>(o => {
            //    o.ForwardClientCertificate = false; 
            // });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env, RolesSeeder seeder)
        {
            CultureInfo culturaPT_BR = new CultureInfo("pt-BR");
            culturaPT_BR.NumberFormat.NumberDecimalSeparator = ".";
            culturaPT_BR.NumberFormat.CurrencyDecimalSeparator = ".";

            IList<CultureInfo> culturaSuportada = new[] { culturaPT_BR };
            app.UseRequestLocalization(new RequestLocalizationOptions()
            {
                DefaultRequestCulture = new RequestCulture(culturaPT_BR),
                SupportedCultures = culturaSuportada,
                SupportedUICultures = culturaSuportada
            });

            if (env.IsDevelopment ())
            {
                app.UseDeveloperExceptionPage ();
                app.UseDatabaseErrorPage ();
            }
            else
            {
                app.UseExceptionHandler ("/Home/Error");
                app.UseHsts ();
            }

            app.UseHttpsRedirection ();
            app.UseStaticFiles (new StaticFileOptions
            {
                OnPrepareResponse = ctx => 
                {
                    ctx.Context.Response.Headers.Add("Cache-Control", "no-cache, no-storage");
                    ctx.Context.Response.Headers.Add("Expires", "-1");
                }
            });

            app.UseCookiePolicy ();
            app.UseAuthentication ();
            
            app.UseMvc (routes =>
            {
                routes.MapRoute (
                    name: "default",
                    template: "{controller=Histograma}/{action=Index}/{id?}");
            });

            seeder.Seed().Wait();
            
        }
    }
}
