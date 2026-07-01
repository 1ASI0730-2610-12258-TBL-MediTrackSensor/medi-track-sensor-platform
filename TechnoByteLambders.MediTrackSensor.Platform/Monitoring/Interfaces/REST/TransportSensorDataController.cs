using Microsoft.AspNetCore.Mvc;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Application.Internal.Commands;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Application.Internal.Queries;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Application.Internal.CommandHandlers;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Interfaces.REST.Resources;

namespace TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Interfaces.REST;

[ApiController]
[Route("api/v1/transports")]
public class TransportSensorDataController : ControllerBase
{
    private readonly EditTransportSensorDataCommandHandler _commandHandler;
    private readonly GetAllTransportsQueryHandler _queryHandler; 

    public TransportSensorDataController(
        EditTransportSensorDataCommandHandler commandHandler,
        GetAllTransportsQueryHandler queryHandler) 
    {
        _commandHandler = commandHandler;
        _queryHandler = queryHandler;
    }

    [HttpPut("{transportId:guid}/sensor-data")]
    public async Task<IActionResult> EditSensorData(Guid transportId, [FromBody] EditTransportSensorDataResource resource)
    {
        var command = new EditTransportSensorDataCommand(transportId, resource.Temperature, resource.Humidity);
        await _commandHandler.HandleAsync(command);
        return Ok(new { message = "Datos del sensor del transporte actualizados con éxito." });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTransports()
    {
        try
        {
            var query = new GetAllTransportsQuery();
            var result = await _queryHandler.HandleAsync(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}