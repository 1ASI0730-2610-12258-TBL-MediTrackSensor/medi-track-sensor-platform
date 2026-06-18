namespace TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Domain.Model.Errors;

public enum MonitoringError
{
    None,
    DeviceNotFound,
    DeviceAlreadyExists,
    DeviceCreationFailed,
    DeviceUpdateFailed,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
