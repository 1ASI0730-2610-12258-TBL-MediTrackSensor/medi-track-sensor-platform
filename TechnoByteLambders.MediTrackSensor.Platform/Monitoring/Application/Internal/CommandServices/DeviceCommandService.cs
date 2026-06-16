using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Application.CommandServices;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Domain.Model.Aggregates;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Domain.Model.Commands;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Domain.Model.Errors;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Domain.Repositories;
using TechnoByteLambders.MediTrackSensor.Platform.Shared.Application.Patterns;
using TechnoByteLambders.MediTrackSensor.Platform.Shared.Domain.Repositories;

namespace TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Application.Internal.CommandServices;

public class DeviceCommandService(
    IDeviceRepository deviceRepository,
    IUnitOfWork unitOfWork) : IDeviceCommandService
{
    public async Task<Result<Device, MonitoringError>> Handle(
        UpdateDeviceSensorDataCommand command,
        CancellationToken cancellationToken = default)
    {
        var device = await deviceRepository.FindByIdAsync(command.Id, cancellationToken);
        if (device is null)
            return new Result<Device, MonitoringError>.Failure(MonitoringError.DeviceNotFound);

        device.UpdateSensorReading(command.SensorReading).UpdateDoorStatus(command.DoorStatus);

        try
        {
            deviceRepository.Update(device);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<Device, MonitoringError>.Success(device);
        }
        catch (Exception)
        {
            return new Result<Device, MonitoringError>.Failure(MonitoringError.DeviceUpdateFailed);
        }
    }
}
