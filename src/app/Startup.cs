using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GTDPad.Services;
using GTDPad.Support;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GTDPad
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            const string authenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            var connectionString = Configuration.GetConnectionString("Main");

            services.AddHttpContextAccessor();

            services.Configure<Settings>(Configuration);

            services.Configure<CookiePolicyOptions>(options => {
                options.CheckConsentNeeded = _ => false;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
                options.HttpOnly = HttpOnlyPolicy.Always;
                options.Secure = CookieSecurePolicy.Always;
            });

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddAuthentication(authenticationScheme)
                .AddCookie(authenticationScheme, options => {
                    options.LoginPath = "/users/login";
                    options.LogoutPath = "/users/logout";
                });

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDateTimeService, DateTimeService>();

            services.AddScoped<IRepository, SqlServerRepository>();

            services.AddControllersWithViews();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }
    }
}
