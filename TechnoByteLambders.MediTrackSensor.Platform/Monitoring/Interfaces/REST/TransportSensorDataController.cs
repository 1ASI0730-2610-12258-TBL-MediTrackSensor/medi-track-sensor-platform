using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    private readonly CreateTransportCommandHandler _createHandler;
    private readonly DeleteTransportCommandHandler _deleteHandler;

    public TransportSensorDataController(
        EditTransportSensorDataCommandHandler commandHandler,
        GetAllTransportsQueryHandler queryHandler,
        CreateTransportCommandHandler createHandler,
        DeleteTransportCommandHandler deleteHandler)
    {
        _commandHandler = commandHandler;
        _queryHandler = queryHandler;
        _createHandler = createHandler;
        _deleteHandler = deleteHandler;
    }

    // Endpoint 3: POST /api/v1/transports
    [HttpPost]
    public async Task<IActionResult> CreateTransport([FromBody] CreateTransportResource resource)
    {
        try
        {
            var command = new CreateTransportCommand(resource.CurrentTemperature, resource.CurrentHumidity);
            var transport = await _createHandler.HandleAsync(command);
            var result = new TransportResource(
                transport.Id,
                transport.CurrentTemperature,
                transport.CurrentHumidity,
                transport.LastSensorUpdate);
            return CreatedAtAction(nameof(GetAllTransports), new { id = transport.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // Endpoint 1: PUT /api/v1/transports/{transportId}/sensor-data
    [HttpPut("{transportId:guid}/sensor-data")]
    public async Task<IActionResult> EditSensorData(Guid transportId, [FromBody] EditTransportSensorDataResource resource)
    {
        try
        {
            var command = new EditTransportSensorDataCommand(transportId, resource.Temperature, resource.Humidity);
            await _commandHandler.HandleAsync(command);
            return Ok(new { message = "Datos del sensor del transporte actualizados con éxito." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // Endpoint 4: DELETE /api/v1/transports/{transportId}
    [HttpDelete("{transportId:guid}")]
    public async Task<IActionResult> DeleteTransport(Guid transportId)
    {
        try
        {
            await _deleteHandler.HandleAsync(new DeleteTransportCommand(transportId));
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // Endpoint 2: GET /api/v1/transports
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