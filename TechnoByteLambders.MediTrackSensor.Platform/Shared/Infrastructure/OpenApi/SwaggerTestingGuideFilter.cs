using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TechnoByteLambders.MediTrackSensor.Platform.Shared.Infrastructure.OpenApi;

/// <summary>
/// Adds a step-by-step testing guide to the Swagger document header.
/// </summary>
public sealed class SwaggerTestingGuideFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        swaggerDoc.Info.Description = """
            ## MediTrack Sensor — REST API v1

            Recursos **anidados** (hijo bajo padre en la URL). JSON en **snake_case**.

            ### Flujo recomendado para probar en Swagger

            1. **POST /api/v1/users** — Registro (email nuevo, role=Admin, entity_name)
            2. **POST /api/v1/users/sign-in** — Login con ese usuario
            3. **GET /api/v1/admins** — Anotar `id` del admin
            4. **POST /api/v1/admins/{adminId}/establishments** — Crear establecimiento
            5. **POST /api/v1/establishments/{establishmentId}/devices** — Crear dispositivo
            6. **PUT .../devices/{deviceId}/sensor-data** — Enviar lecturas de sensores

            Cada endpoint incluye **descripción**, **ejemplos en parámetros de ruta** y **body de ejemplo** (POST/PUT).
            Pulsa **Try it out** → **Execute** para probar.
            """;
    }
}
