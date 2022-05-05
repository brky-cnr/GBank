using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MediatR;
using GBank.Api.Extensions;
using GBank.Domain.Interfaces;
using GBank.Domain.Settings;
using GBank.Infrastructure.Helpers;
using GBank.Infrastructure.Repositories;
using GBank.Infrastructure.Settings;
using GBank.Api.Application.Customers.Commands;
using GBank.Api.Application.Accounts.Commands;
using GBank.Api.Application.Transactions.Commands;
using GBank.Api.Application.Accounts.Queries;
using GBank.Api.Application.Transactions.Queries;

namespace GBank.Api
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
            services.Configure<MongoDBSettings>(Configuration.GetSection("MongoDBSettings"));
            services.AddSingleton<IMongoDBSettings>(serviceProvider => serviceProvider.GetRequiredService<IOptions<MongoDBSettings>>().Value);
            services.AddMediatR(typeof(RegisterCustomerCommand).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(RegisterAccountCommand).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(PlaceAccountTransactionCommand).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(GetAccountQuery).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(GetCustomerTransactionsQuery).GetTypeInfo().Assembly);
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IAccountRepository, AccoutRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IEventLogRepository, EventLogRepository>();

            #region Jwt

            var key = Encoding.ASCII.GetBytes("GBankDBSecretKey");

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context => Task.CompletedTask
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(5)
                };
            });

            #endregion

            services.AddHealthChecks();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GBank Api", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                 {
                   new OpenApiSecurityScheme
                   {
                     Reference = new OpenApiReference
                     {
                       Type = ReferenceType.SecurityScheme,
                       Id = "Bearer"
                     }
                    },
                    new string[] { }
                  }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GBank API V1");
            });

            DBSeed.Seed(app);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.ConfigureExceptionHandler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/api/status");
                endpoints.MapControllers();
            });
        }
    }
}
