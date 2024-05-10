using NexumTech.Domain.Interfaces;
using NexumTech.Infra.DAO.Interfaces;
using NexumTech.Infra.DAO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NexumTech.Domain.Services;
using NexumTech.Infra.API;
using NexumTech.Infra.API.Interfaces;
using NexumTech.Infra.WEB;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using NexumTech.Infra.API.Middlewares;

namespace NexumTech.API
{
    public class Startup : IStartup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:5230", "http://191.232.39.95");
                    });
            });

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddHttpClient();

            services.Configure<AppSettingsAPI>(Configuration);

            services.AddScoped<AppSettingsAPI>();

            #region Domain Dependency Injection

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMailMessageService, MailMessageService>();

            #endregion

            #region Infra Dependency Injection

            services.AddScoped<IUserDAO, UserDAO>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<ITokenService, TokenService>();

            #endregion

            #region Base Database Service Dependency Injection

            services.AddScoped<BaseDatabaseService>();

            #endregion

            services.AddControllers();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JWT:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,

                };
            });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public void Configure(WebApplication app, IWebHostEnvironment enviroment)
        {
            app.UseMiddleware<CultureMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            var supportedCultures = new[]
            {
                 new CultureInfo("en-US"),
                 new CultureInfo("pt-BR"),
                 new CultureInfo("es-ES")
            };

            app.UseCors("AllowSpecificOrigins");

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();
        }
    }

    public interface IStartup
    {
        IConfiguration Configuration { get; }
        void ConfigureServices(IServiceCollection services);
        void Configure(WebApplication app, IWebHostEnvironment enviroment);
    }

    public static class StartupExtensions
    {
        public static WebApplicationBuilder UseStartup<TStartup>(this WebApplicationBuilder WebAppBuilder) where TStartup : IStartup
        {
            var startup = Activator.CreateInstance(typeof(TStartup), WebAppBuilder.Configuration) as IStartup;
            if (startup == null) throw new ArgumentException("Invalid Startup.cs file");


            startup.ConfigureServices(WebAppBuilder.Services);

            var app = WebAppBuilder.Build();
            startup.Configure(app, app.Environment);

            app.Run();

            return WebAppBuilder;
        }


    }

}
