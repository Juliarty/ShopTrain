using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore.SqlServer;
using Shop.Database;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Microsoft.AspNetCore.Identity;
using Shop.Application.Users;
using Shop.Application;
using Shop.Application.Infrastructure;
using Shop.UI.Infrastructure;

namespace Shop.UI
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
            services.AddHttpContextAccessor();
            // Look at GDPR (General Data Protection Regulation)
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });


            // it is scoped, so each ctx object lives over the request lifetime 
            services.AddDbContext<ApplicationDbContext>((options) => options.UseSqlServer(Configuration["DefaultConnection"]));
            
            // AddDefaultIdentity adds a bunch of pre-built stuff (boilerplates) like LoginPartial.cshtml and the like
            services.AddIdentity<IdentityUser, IdentityRole>(
                options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;


                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.DefaultLockoutTimeSpan = new TimeSpan(0, 30, 0);
                })
                .AddEntityFrameworkStores<ApplicationDbContext>();         ///Wow!

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Accounts/Login/";
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim("Role", "Admin"));
                // options.AddPolicy("Manager", policy => policy.RequireClaim("Role", "Manager"));
                options.AddPolicy("Manager", 
                    policy => policy
                    .RequireAssertion(context => 
                    context.User.HasClaim(x => x.Value == "Admin") || 
                    context.User.HasClaim(x => x.Value == "Manager")));
            });

            services.AddSession();
            services.AddAntiforgery(options =>
            {
                // Set Cookie properties using CookieBuilder properties†.
                options.Cookie.Name = "LOL";
                options.FormFieldName = "AntiforgeryFieldname";
                options.HeaderName = "X-CSRF-TOKEN-HEADERNAME";
                options.SuppressXFrameOptionsHeader = false;
            });

            services.AddRazorPages()
                .AddXmlSerializerFormatters()
                .AddRazorPagesOptions(options => 
                {
                    options.Conventions.AuthorizeFolder("/Admin");
                    options.Conventions.AuthorizePage("/Admin/ConfigureUsers", "Admin");
                });
            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));

            services.AddTransient<ISessionManager, SessionManager>();
            services.AddApplicationServices();
            services.AddHttpContextAccessor();
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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
            });
            //Todo: look at GDPR
            app.UseCookiePolicy();

            StripeConfiguration.ApiKey = Configuration.GetSection("Stripe")["SecretKey"];

        }
    }
}
