using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using NexumTech.Infra.API;
using NexumTech.Infra.WEB;
using NexumTech.Infra.WEB.Middlewares;
using System.Globalization;

namespace NexumTech.Web
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
            services.AddControllersWithViews()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("pt-BR"),
                    new CultureInfo("en-US"),
                    new CultureInfo("es-ES"),                   
                };

                options.DefaultRequestCulture = new RequestCulture("pt-BR");
                options.SupportedUICultures = supportedCultures;
            });

            services.AddHttpClient();

            #region WEB appsettings.json Dependency Injection

            services.Configure<AppSettingsWEB>(Configuration);

            services.AddScoped<AppSettingsWEB>();

            #endregion

            #region Base Http Service Dependency Injection

            services.AddScoped<BaseHttpService>();

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRequestLocalization();

            app.UseMiddleware<CultureMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });
        }
    }
}
