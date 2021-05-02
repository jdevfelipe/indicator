using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using indicator_api.Context;
using indicator_api.Exceptions;
using indicator_api.Jobs;
using indicator_api.MailSender;
using indicator_api.Repositories;
using indicator_api.Repositories.Interfaces;
using indicator_api.Services;
using indicator_api.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace indicator_api
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
            services.AddCors();
            services.AddControllers();
            var key = Encoding.ASCII.GetBytes(Settings.Settings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddControllers().AddNewtonsoftJson(options =>
options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

            var host = Configuration["DBHOST"] ?? "";
            var port = Configuration["DBPORT"] ?? "";
            var password = Configuration["DBPASSWORD"] ?? "";

            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql($"server={host};userid=;pwd={password};"
                    + $"port={port};database="));
            services.AddControllers();

            services.AddScoped<UserService>();
            services.AddScoped<UserRepository>();
            services.AddScoped<IndicatorService>();
            services.AddScoped<IndicatorRepository>();
            services.AddScoped<CompanyRepository>();
            services.AddScoped<CompanyService>();
            services.AddScoped<ProductRepository>();
            services.AddScoped<ProductService>();
            services.AddScoped<PaymentAccountRepository>();
            services.AddScoped<IndicationService>();
            services.AddScoped<IndicationRepository>();
            services.AddScoped<PaymentService>();
            services.AddScoped<PaymentRepository>();
            services.AddSingleton<SendPaymentMail>();
            services.AddSingleton<SendForgotPassEmail>();
            services.AddScoped<SendConfirmAccountCode>();
            services.AddScoped<CpfValidate>();
            services.AddScoped<CnpjValidate>();
            services.AddScoped<SendContactMail>();

            //services.AddCronJob<NewPaymentJob>(c =>
            //{
            //    c.TimeZoneInfo = TimeZoneInfo.Local;
            //    c.CronExpression = @"*/5 * * * *";
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            // Hook in the global error-handling middleware
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            //app.UseHttpsRedirection();

            app.UseRouting();


            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
