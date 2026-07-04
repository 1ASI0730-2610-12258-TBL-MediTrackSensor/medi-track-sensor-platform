using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Application.CommandServices;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Domain.Model.Errors;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Domain.Repositories;
using TechnoByteLambders.MediTrackSensor.Platform.Shared.Application.Patterns;
using TechnoByteLambders.MediTrackSensor.Platform.Shared.Domain.Repositories;

namespace TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Application.Internal.CommandServices;

public class DeviceCommandService(
    IDeviceRepository deviceRepository,
    IUnitOfWork unitOfWork) : IDeviceCommandService
{
    public async Task<Result<bool, string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var device = await deviceRepository.FindByIdAsync(id, cancellationToken);
        if (device is null)
            return new Result<bool, string>.Failure(MonitoringErrors.DeviceNotFound.Description);

        deviceRepository.Remove(device);
        await unitOfWork.CompleteAsync(cancellationToken);
        return new Result<bool, string>.Success(true);
    }
}
