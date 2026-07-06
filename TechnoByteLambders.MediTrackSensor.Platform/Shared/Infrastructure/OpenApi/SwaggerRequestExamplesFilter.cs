using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TechnoByteLambders.MediTrackSensor.Platform.Shared.Infrastructure.OpenApi;

/// <summary>
/// Adds a ready-to-try JSON example body for each POST/PUT endpoint in Swagger UI.
/// </summary>
public sealed class SwaggerRequestExamplesFilter : IOperationFilter
{
    private static readonly Dictionary<string, string> Examples = new(StringComparer.OrdinalIgnoreCase)
    {
        ["POST:/api/v1/users"] = """
            {
              "name": "María García",
              "dni": "12345678",
              "email": "nuevo.admin@clinica.com",
              "phone": "+51999999999",
              "job_title": "Administrador",
              "entry_date": "2026-07-06",
              "role": "Admin",
              "password": "Secret123!",
              "photo": "",
              "entity_name": "Clínica San Martín"
            }
            """,
        ["POST:/api/v1/users/sign-in"] = """
            {
              "email": "pilsen@gmail.com",
              "password": "tu_password"
            }
            """,
        ["POST:/api/v1/admins"] = """
            {
              "entity_name": "Clínica San Martín",
              "entity_code": "ENT-1",
              "schedule": "Lun-Vie 8:00-18:00",
              "user_id": 1
            }
            """,
        ["POST:/api/v1/admins/{adminid}/establishments"] = """
            {
              "establishment_name": "Farmacia Central",
              "establishment_type": "Pharmacy",
              "address": "Av. Principal 123",
              "district": "Miraflores",
              "city_region": "Lima",
              "country": "PE",
              "latitude": -12.1201,
              "longitude": -77.0342,
              "phone": "+51988887777",
              "email": "farmacia@clinica.com",
              "website": "https://farmacia.clinica.com"
            }
            """,
        ["POST:/api/v1/establishments/{establishmentid}/operators"] = """
            {
              "schedule": "Lun-Sáb 9:00-17:00",
              "users_id": 2
            }
            """,
        ["PUT:/api/v1/establishments/{establishmentid}/operators/{operatorid}"] = """
            {
              "schedule": "Lun-Vie 8:00-16:00"
            }
            """,
        ["POST:/api/v1/establishments/{establishmentid}/devices"] = """
            {
              "exact_location": "Cámara fría - Estante 3",
              "type_of_medication": "Refrigerated",
              "enabled_sensors": "{\"temperature\":true,\"humidity\":true,\"door\":true}"
            }
            """,
        ["PUT:/api/v1/establishments/{establishmentid}/devices/{deviceid}/sensor-data"] = """
            {
              "temperature": 4.2,
              "humidity": 55.0,
              "light_intensity": 120.5,
              "air_quality": 98.0,
              "vibration": 0.1,
              "atmospheric_pressure": 1013.25,
              "suspended_particles": 12.0,
              "door_status": "Closed"
            }
            """,
        ["POST:/api/v1/establishments/{establishmentid}/transports"] = """
            {
              "type_of_transport": "Refrigerated",
              "type_of_medication": "Refrigerated",
              "enabled_sensors": "{\"temperature\":true,\"humidity\":true,\"door\":true}"
            }
            """,
        ["PUT:/api/v1/establishments/{establishmentid}/transports/{transportid}/sensor-data"] = """
            {
              "temperature": 3.8,
              "humidity": 60.0,
              "light_intensity": 80.0,
              "air_quality": 95.0,
              "vibration": 0.3,
              "atmospheric_pressure": 1012.0,
              "suspended_particles": 8.0,
              "door_status": "Closed"
            }
            """,
        ["POST:/api/v1/admins/{adminid}/subscriptions"] = """
            {
              "plan": "Premium",
              "start_date": "2026-07-01",
              "end_date": "2027-06-30"
            }
            """
    };

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var method = context.ApiDescription.HttpMethod ?? "";
        var path = "/" + (context.ApiDescription.RelativePath ?? "").TrimStart('/');
        var key = $"{method}:{path}";

        if (!Examples.TryGetValue(key, out var json))
            return;

        operation.RequestBody ??= new OpenApiRequestBody
        {
            Required = true,
            Content = new Dictionary<string, OpenApiMediaType>()
        };

        if (!operation.RequestBody.Content.ContainsKey("application/json"))
            operation.RequestBody.Content["application/json"] = new OpenApiMediaType();

        operation.RequestBody.Content["application/json"].Example =
            OpenApiAnyFactory.CreateFromJson(json.Trim());
    }
}
