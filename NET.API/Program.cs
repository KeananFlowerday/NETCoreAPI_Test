using NET.API.Models;
using NET.API.Handlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;

var logger = LogManager.Setup().LoadConfigurationFromFile(Constants.NLOG).GetCurrentClassLogger();

try
{

    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Add services to the container.
    //builder.Services.AddScoped<IIntegrationTypeRepository, IntegrationTypeRepository>();
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.AddSecurityDefinition(Constants.API_KEY_HEADER, new OpenApiSecurityScheme
        {
            Description = "ApiKey must appear in header",
            Type = SecuritySchemeType.ApiKey,
            Name = Constants.API_KEY_HEADER,
            In = ParameterLocation.Header,
            Scheme = "ApiKeyScheme"
        });
        var key = new OpenApiSecurityScheme()
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = Constants.API_KEY_HEADER
            },
            In = ParameterLocation.Header
        };
        var requirement = new OpenApiSecurityRequirement
        {
            { key, new List<string>() }
        };
        c.AddSecurityRequirement(requirement);
    });

    builder.Services.AddAuthentication()
        .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(Constants.APIKEY, options => { });

    var app = builder.Build();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception exception)
{
    logger.Error(exception, "Stopped API because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}