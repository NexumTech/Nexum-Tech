﻿using Nexum_Tech.Domain.Interfaces;
using Nexum_Tech.Infra.DAO.Interfaces;
using Nexum_Tech.Infra.DAO;
using NexumTech.Domain.Services;

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

            services.AddHttpClient();

            #region Domain Dependency Injection

            services.AddScoped<ITest, TestService>();

            #endregion

            #region DAO Dependency Injection

            services.AddScoped<ITestDAO, TestDAO>();

            #endregion

            #region Base Database Service Dependency Injection

            services.AddScoped<BaseDatabaseService>();

            #endregion

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public void Configure (WebApplication app, IWebHostEnvironment enviroment)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowSpecificOrigins");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
        }
    }

    public interface IStartup
    {
        IConfiguration Configuration { get; }
        void ConfigureServices (IServiceCollection services);
        void Configure (WebApplication app, IWebHostEnvironment enviroment);
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