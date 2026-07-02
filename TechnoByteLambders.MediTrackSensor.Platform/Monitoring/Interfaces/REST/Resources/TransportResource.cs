namespace TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Interfaces.REST.Resources;

public record TransportResource(Guid Id, decimal CurrentTemperature, decimal CurrentHumidity, DateTime LastSensorUpdate);