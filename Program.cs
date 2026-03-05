using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QueueProject.Application.Common.Interfaces;
using QueueProject.Application.Queues.Queries;
using QueueProject.Application.Queues.Queries.GetQueuesQuery;
using QueueProject.Infrastructure.Auth;
using QueueProject.Infrastructure.Persistence;
using QueueProject.Infrastructure.Settings;
using Serilog;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Debug()
//    .WriteTo.Console()
//    .WriteTo.File("C:/App/QueueProjectLogs/log.txt", rollingInterval: RollingInterval.Day)
//    .CreateLogger();

namespace QueueProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

            // Add services to the container.

            builder.Services.AddControllers();
            
            //Jwt configuration
            builder.Services.Configure<JwtSettings>(
            builder.Configuration.GetSection(nameof(JwtSettings)));

            var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                    };
                });
            
            builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

            // Configure MongoDB
            builder.Services.Configure<MongoDbSettings>(
                builder.Configuration.GetSection(nameof(MongoDbSettings)));

            builder.Services.AddSingleton<IMongoDbContext, MongoDbContext>();

            // Add MediatR
            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(GetQueuesQuery).Assembly));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            // Add Swagger
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Queue Project API",
                    Version = "v1",
                    Description = "A queue management service for handling basic queue operations"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Enter JWT token: Bearer {your token}",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
