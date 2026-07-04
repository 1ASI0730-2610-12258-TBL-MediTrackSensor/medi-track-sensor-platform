using TechnoByteLambders.MediTrackSensor.Platform.Shared.Application.Patterns;

namespace TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Application.CommandServices;

public interface IDeviceCommandService
{
    Task<Result<bool, string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
