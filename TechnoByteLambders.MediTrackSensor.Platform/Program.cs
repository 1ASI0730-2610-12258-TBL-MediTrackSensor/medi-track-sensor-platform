using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
// IAM
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Application.CommandServices;
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Application.Internal.CommandServices;
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Application.Internal.OutboundServices;
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Domain.Repositories;
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Infrastructure.OutboundServices;
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Infrastructure.Persistence.EFC.Repositories;
// Establishments
using TechnoByteLambders.MediTrackSensor.Platform.Establishments.Application.CommandServices;
using TechnoByteLambders.MediTrackSensor.Platform.Establishments.Application.Internal.CommandServices;
using TechnoByteLambders.MediTrackSensor.Platform.Establishments.Application.Internal.QueryServices;
using TechnoByteLambders.MediTrackSensor.Platform.Establishments.Application.QueryServices;
using TechnoByteLambders.MediTrackSensor.Platform.Establishments.Domain.Repositories;
using TechnoByteLambders.MediTrackSensor.Platform.Establishments.Infrastructure.Persistence.EFC.Repositories;
// Monitoring
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Application.CommandServices;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Application.Internal.CommandServices;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Domain.Repositories;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Infrastructure.Persistence.EFC.Repositories;
// Subscriptions
using TechnoByteLambders.MediTrackSensor.Platform.Subscriptions.Application.CommandServices;
using TechnoByteLambders.MediTrackSensor.Platform.Subscriptions.Application.Internal.CommandServices;
using TechnoByteLambders.MediTrackSensor.Platform.Subscriptions.Domain.Repositories;
using TechnoByteLambders.MediTrackSensor.Platform.Subscriptions.Infrastructure.Persistence.EFC.Repositories;
// Shared
using TechnoByteLambders.MediTrackSensor.Platform.Shared.Domain.Repositories;
using TechnoByteLambders.MediTrackSensor.Platform.Shared.Infrastructure.Interfaces.ASP.Configuration;
using TechnoByteLambders.MediTrackSensor.Platform.Shared.Infrastructure.Persistence.EFC.Configuration;
using TechnoByteLambders.MediTrackSensor.Platform.Shared.Infrastructure.Persistence.EFC.Repositories;
using TechnoByteLambders.MediTrackSensor.Platform.Shared.Infrastructure.Pipeline.Middleware.Extensions;
using TechnoByteLambders.MediTrackSensor.Platform.Shared.Resources;
using TechnoByteLambders.MediTrackSensor.Platform.Shared.Resources.Errors;
using ProblemDetailsFactory = TechnoByteLambders.MediTrackSensor.Platform.Shared.Interfaces.REST.ProblemDetails.ProblemDetailsFactory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()))
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.SnakeCaseLower;
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    })
    .AddDataAnnotationsLocalization();

builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",
                "http://localhost:4173",
                "https://medi-track-sensor-frontend.vercel.app"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddSingleton<IStringLocalizer<ErrorMessages>, StringLocalizer<ErrorMessages>>();
builder.Services.AddSingleton<IStringLocalizer<CommonMessages>, StringLocalizer<CommonMessages>>();
builder.Services.AddSingleton<ProblemDetailsFactory>();

// Database
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    string connectionString;

    var host = Environment.GetEnvironmentVariable("DATABASE_HOST");
    var port = Environment.GetEnvironmentVariable("DATABASE_PORT");
    var database = Environment.GetEnvironmentVariable("DATABASE_NAME");
    var user = Environment.GetEnvironmentVariable("DATABASE_USER");
    var password = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");

    if (!string.IsNullOrEmpty(host) && !string.IsNullOrEmpty(port) && !string.IsNullOrEmpty(database) && !string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(password))
    {
        connectionString = $"server={host};port={port};database={database};user={user};password={password}";
    }
    else
    {
        connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        connectionString = Environment.ExpandEnvironmentVariables(connectionString);
    }

    if (string.IsNullOrWhiteSpace(connectionString))
        throw new InvalidOperationException("Database connection string is not set in the configuration.");

    options.UseMySQL(connectionString)
        .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
        .EnableDetailedErrors();

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});

// Shared
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// IAM
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// Monitoring
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<IDeviceCommandService, DeviceCommandService>();

// Establishments
builder.Services.AddScoped<IEstablishmentRepository, EstablishmentRepository>();
builder.Services.AddScoped<IEstablishmentCommandService, EstablishmentCommandService>();
builder.Services.AddScoped<IOperatorRepository, OperatorRepository>();
builder.Services.AddScoped<IOperatorCommandService, OperatorCommandService>();
builder.Services.AddScoped<IOperatorQueryService, OperatorQueryService>();

// Subscriptions
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<ISubscriptionCommandService, SubscriptionCommandService>();

var app = builder.Build();

// Database Migration/Creation
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate(); 
}

app.UseGlobalExceptionHandler();

var supportedCultures = new[] { "en", "es" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
app.UseRequestLocalization(localizationOptions);

app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "MediTrack Sensor API v1");
});
app.UseHttpsRedirection();
app.UseCors("FrontendPolicy");
app.UseAuthorization();
app.MapControllers();
app.Run();