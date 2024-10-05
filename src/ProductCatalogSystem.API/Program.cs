using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using ProductCatalogSystem;
using ProductCatalogSystem.API;
using ProductCatalogSystem.API.ServiceExtension;
using ProductCatalogSystem.Core.CustomAttributes;
using ProductCatalogSystem.Core.Interfaces;
using ProductCatalogSystem.Core.Services;
using ProductCatalogSystem.Entities;
using ProductCatalogSystem.Repositories;
using Serilog;
using Serilog.Events;
using AuthenticationService = ProductCatalogSystem.Core.Services.AuthenticationService;
using IAuthenticationService = ProductCatalogSystem.Core.Interfaces.IAuthenticationService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("database"));
//builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConn")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddMemoryCache();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(Automapper));
builder.Services.ConfigureBadRequest();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwaggerGen();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IProductService, ProductService>();

builder.Services.ConfigureSerilog(builder);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Host.UseSerilog((context, services, configuration) => configuration
.ReadFrom.Configuration(context.Configuration)
.ReadFrom.Services(services)
.Enrich.FromLogContext(), writeToProviders: false);
builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SchemaFilter<SwaggerSchemaExampleFilter>();
});

var app = builder.Build();

//For test only
AppDbInitializer.Seed(app);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseSerilogRequestLogging(options =>
{
    /**Customize the message template
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

    Emit debug-level events instead of the defaults
    options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Debug;
    Attach additional properties to the request completion event
    */
    options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Debug;
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
    };
});

app.MapControllers();

app.Run();

