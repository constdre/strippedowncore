using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ASPracticeCore
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false; //true to enable cookie consent
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            
            //sets default redirection page when user is not authenticated:
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => { options.LoginPath = "/Areas/Accounts/Views/Access/Login/"; });

            services.AddDistributedMemoryCache();

            services.AddHttpContextAccessor();

            //Does the session handle simultaneous users?
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true; //make this false to enable Check Consent
            });


            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            services.AddDbContext<DAL.ApplicationContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("ROG_ASPracticeCore"));
            });

            services.AddControllersWithViews();
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            /*
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name:"Area",
                    template:"{area:exists}/{controller=Home}/{action=Index}/{id?}");
            });
            */
            app.UseEndpoints(endPoints =>
            {
                endPoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}");
                endPoints.MapAreaControllerRoute(name: "areas", "areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            });
        }
    }
}
