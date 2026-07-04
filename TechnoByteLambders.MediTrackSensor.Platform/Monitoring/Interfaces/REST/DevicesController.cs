using Microsoft.AspNetCore.Mvc;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Application.CommandServices;

namespace TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Interfaces.REST;

[ApiController]
[Route("api/v1/devices")]
public class DevicesController(IDeviceCommandService commandService) : ControllerBase
{
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await commandService.DeleteAsync(id, ct);
        if (result.IsFailure) return NotFound(new { error = ((dynamic)result).Error });
        return NoContent();
    }
}
