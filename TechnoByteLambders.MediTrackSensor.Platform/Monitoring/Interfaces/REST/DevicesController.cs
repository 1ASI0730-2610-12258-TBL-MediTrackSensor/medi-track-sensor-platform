using Microsoft.AspNetCore.Mvc;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Application.CommandServices;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Domain.Model.Commands;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Domain.Model.Errors;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Domain.Model.ValueObjects;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Interfaces.REST.Resources;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Interfaces.REST.Transform;
using TechnoByteLambders.MediTrackSensor.Platform.Shared.Application.Patterns;
using ProblemDetailsFactory = TechnoByteLambders.MediTrackSensor.Platform.Shared.Interfaces.REST.ProblemDetails.ProblemDetailsFactory;

namespace TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Interfaces.REST;

[ApiController]
[Route("api/v1/devices")]
public class DevicesController(
    IDeviceCommandService deviceCommandService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPut("{id:int}/sensor-data")]
    public async Task<IActionResult> UpdateSensorData(
        int id,
        [FromBody] UpdateDeviceSensorDataResource resource,
        CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<DoorStatus>(resource.DoorStatus, true, out var doorStatus))
        {
            return problemDetailsFactory.CreateProblemDetails(
                this,
                StatusCodes.Status400BadRequest,
                MonitoringError.DeviceUpdateFailed,
                "Invalid DoorStatus value.");
        }

        var reading = new SensorReading(
            resource.Temperature,
            resource.Humidity,
            resource.LightIntensity,
            resource.AirQuality,
            resource.Vibration,
            resource.AtmosphericPressure,
            resource.SuspendedParticles);

        var result = await deviceCommandService.Handle(
            new UpdateDeviceSensorDataCommand(id, reading, doorStatus),
            cancellationToken);

        return result switch
        {
            Result<Domain.Model.Aggregates.Device, MonitoringError>.Success success =>
                Ok(DeviceResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),
            Result<Domain.Model.Aggregates.Device, MonitoringError>.Failure failure =>
                failure.Error switch
                {
                    MonitoringError.DeviceNotFound => problemDetailsFactory.CreateProblemDetails(
                        this,
                        StatusCodes.Status404NotFound,
                        MonitoringError.DeviceNotFound,
                        MonitoringErrors.DeviceNotFound.Description),
                    MonitoringError.DeviceUpdateFailed => problemDetailsFactory.CreateProblemDetails(
                        this,
                        StatusCodes.Status400BadRequest,
                        MonitoringError.DeviceUpdateFailed,
                        MonitoringErrors.DeviceUpdateFailed.Description),
                    _ => problemDetailsFactory.CreateProblemDetails(
                        this,
                        StatusCodes.Status500InternalServerError,
                        MonitoringError.InternalServerError,
                        MonitoringErrors.InternalServerError.Description)
                },
            _ => problemDetailsFactory.CreateProblemDetails(
                this,
                StatusCodes.Status500InternalServerError,
                MonitoringError.InternalServerError,
                MonitoringErrors.InternalServerError.Description)
        };
    }
}
