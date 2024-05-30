using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using NexumTech.Infra.API;
using NexumTech.Infra.WEB;
using NexumTech.Infra.WEB.Middlewares;
using NexumTech.Web.Services;
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
                options.SupportedCultures = supportedCultures;
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

            #region Historical Chart Service Dependency Injection

            services.AddScoped<IHistoricalChartService, HistoricalChartService>();

            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            app.UseMiddleware<CultureMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
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
