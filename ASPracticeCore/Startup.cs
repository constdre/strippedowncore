using System;
using System.IO;
using System.Linq;
using System.Text;
using ASPracticeCore.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace ASPracticeCore
{
    public class Startup
    {
        IWebHostEnvironment _environment;
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //sets default redirection page when user is not authenticated:
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = Constants.COOKIE_NAME_AUTH;
                    options.LoginPath = new PathString("/Accounts/Access/Login");
                    options.AccessDeniedPath = new PathString("/Home/Error");
                    options.Cookie.SecurePolicy = _environment.IsDevelopment() ? CookieSecurePolicy.None : CookieSecurePolicy.Always;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(40);


                });
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false; //true to enable cookie consent 
                options.MinimumSameSitePolicy = SameSiteMode.None;//default on session cookie is lax, basically means Cookie.SameSite won't be affected.

            });
            services.AddDistributedMemoryCache();


            services.AddSession(options =>
            {
                //Session works with a cookie that contains the session ID
                options.Cookie.Name = Constants.COOKIE_NAME_SESSION;
                options.IdleTimeout = TimeSpan.FromMinutes(20); //default too
                options.Cookie.HttpOnly = true; //default
                options.Cookie.IsEssential = true; //make this false to enable Check Consent
            });

            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //what does this do again

            services.AddDataProtection();
            services.AddControllersWithViews(config =>
            {
                //options.EnableEndpointRouting = false; //to allow for standard mvc routes

                //Require authentication on all controllers by default, just add [AllowAnonymous] on exclusions.
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
                config.Filters.Add(new AuthorizeFilter(policy));

            }).AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);//for classes with ref-type properties


            services.AddDbContext<DAL.ApplicationContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("ROG_ASPracticeCore"));
                options.UseLazyLoadingProxies();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
                //app.Run(async (context) =>
                //{
                //    var sb = new StringBuilder();
                //    var nl = System.Environment.NewLine;
                //    var rule = string.Concat(nl, new string('-', 40), nl);
                //    var authSchemeProvider = app.ApplicationServices
                //        .GetRequiredService<IAuthenticationSchemeProvider>();

                //    sb.Append($"Request{rule}");
                //    sb.Append($"{DateTimeOffset.Now}{nl}");
                //    sb.Append($"{context.Request.Method} {context.Request.Path}{nl}");
                //    sb.Append($"Scheme: {context.Request.Scheme}{nl}");
                //    sb.Append($"Host: {context.Request.Headers["Host"]}{nl}");
                //    sb.Append($"PathBase: {context.Request.PathBase.Value}{nl}");
                //    sb.Append($"Path: {context.Request.Path.Value}{nl}");
                //    sb.Append($"Query: {context.Request.QueryString.Value}{nl}{nl}");

                //    sb.Append($"Connection{rule}");
                //    sb.Append($"RemoteIp: {context.Connection.RemoteIpAddress}{nl}");
                //    sb.Append($"RemotePort: {context.Connection.RemotePort}{nl}");
                //    sb.Append($"LocalIp: {context.Connection.LocalIpAddress}{nl}");
                //    sb.Append($"LocalPort: {context.Connection.LocalPort}{nl}");
                //    sb.Append($"ClientCert: {context.Connection.ClientCertificate}{nl}{nl}");

                //    sb.Append($"Identity{rule}");
                //    sb.Append($"User: {context.User.Identity.Name}{nl}");
                //    var scheme = await authSchemeProvider
                //        .GetSchemeAsync(IISDefaults.AuthenticationScheme);
                //    sb.Append($"DisplayName: {scheme?.DisplayName}{nl}{nl}");

                //    sb.Append($"Headers{rule}");
                //    foreach (var header in context.Request.Headers)
                //    {
                //        sb.Append($"{header.Key}: {header.Value}{nl}");
                //    }
                //    sb.Append(nl);

                //    sb.Append($"Websockets{rule}");
                //    if (context.Features.Get<IHttpUpgradeFeature>() != null)
                //    {
                //        sb.Append($"Status: Enabled{nl}{nl}");
                //    }
                //    else
                //    {
                //        sb.Append($"Status: Disabled{nl}{nl}");
                //    }

                //    sb.Append($"Configuration{rule}");
                //    foreach (var pair in Configuration.AsEnumerable())
                //    {
                //        sb.Append($"{pair.Key}: {pair.Value}{nl}");
                //    }
                //    sb.Append(nl);

                //    sb.Append($"Environment Variables{rule}");
                //    var vars = System.Environment.GetEnvironmentVariables();
                //    foreach (var key in vars.Keys.Cast<string>().OrderBy(key => key,
                //        StringComparer.OrdinalIgnoreCase))
                //    {
                //        var value = vars[key];
                //        sb.Append($"{key}: {value}{nl}");
                //    }

                //    context.Response.ContentType = "text/plain";
                //    await context.Response.WriteAsync(sb.ToString());
                //});
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles")),
                RequestPath = "/StaticFiles"
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "clientapp\\dist")),
                RequestPath = "/clientapp/dist"
            });
            //app.UseHttpContextItemsMiddleware(); //can't be found

            app.UseRouting();


            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endPoints =>
            {
                endPoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endPoints.MapControllerRoute(
                    name: "default_area",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                
                endPoints.MapControllers(); //for Route attributed actions
            });


            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");

            //    routes.MapRoute(
            //        name: "Area",
            //        template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            //});

        }
    }
}
