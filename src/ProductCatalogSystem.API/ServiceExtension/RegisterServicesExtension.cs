
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Core;
using Serilog.Enrichers;
using Serilog.Events;
using System.Net;
using System.Text;
using ProductCatalogSystem.Core.Entities;
using ProductCatalogSystem.Entities;
using ProductCatalogSystem.Core.CustomAttributes;

namespace ProductCatalogSystem.API.ServiceExtension
{
    public static class RegisterServicesExtension
    {
        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            //var secretKey = configuration.GetSection("SECRET");
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["validIssuer"],
                    ValidAudience = jwtSettings["validAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SECRET"]))
                };
            });
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);

                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        }
        public static void ConfigureBadRequest(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(a =>
            {
                a.InvalidModelStateResponseFactory = context =>
                {

                    return new BadRequestObjectResult(new CustomBadRequest(context))
                    {
                        ContentTypes = { "application /json", "application/xml" },
                    };
                };
            });
        }
        public static void ConfigureSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product catalog service API", Version = "v1" });
                c.EnableAnnotations();
                c.SchemaFilter<SwaggerSchemaExampleFilter>();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
        }
        public static void ConfigureSerilog(this IServiceCollection services, WebApplicationBuilder builder)
        {
            var config = builder.Configuration;
            string seqCon = config.GetValue<string>("Serilog:SeqUrl");
            string apiKey = config.GetValue<string>("Serilog:SeqApiKey");
            string logFilePath = config.GetSection("Serilog:LogPath").Value;
            string logLevel = config.GetValue<string>("Serilog:LogLevel");
            var levelSwitch = new LoggingLevelSwitch() { MinimumLevel = (LogEventLevel)Enum.Parse(typeof(LogEventLevel), logLevel) };
            int retainCount = config.GetValue<int>("Serilog:FileRetainCount");
            int fileSize = config.GetValue<int>("Serilog:FileSize");
            const string loggerTemplate = @"{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u4}]<{ThreadId}> [{SourceContext:l}] {Message:lj}{NewLine}{Exception}";
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (source, certificate, chain, sslPolicyError) => true;
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            builder.Host.UseSerilog((ctx, lc) => lc
                                   .Enrich.FromLogContext()
                                   .Enrich.WithProperty("MachineName", Environment.MachineName)
                                   .Enrich.WithProperty("Application", "ProductCatalogSystem")
                                   .Enrich.With(new ThreadIdEnricher())
                                   .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                                   .MinimumLevel.ControlledBy(levelSwitch)
                                   .WriteTo.Console(outputTemplate: loggerTemplate)
                                   .WriteTo.Seq(seqCon, apiKey: apiKey, messageHandler: httpClientHandler, eventBodyLimitBytes: null)
                                   .WriteTo.File(logFilePath, outputTemplate: loggerTemplate, rollOnFileSizeLimit: true, fileSizeLimitBytes: fileSize, retainedFileCountLimit: retainCount));
        }

    }


}
