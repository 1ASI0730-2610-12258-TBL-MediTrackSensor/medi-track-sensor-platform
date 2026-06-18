using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Domain.Model.Aggregates;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Interfaces.REST.Resources;

namespace TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Interfaces.REST.Transform;

public static class DeviceResourceFromEntityAssembler
{
    public static DeviceResource ToResourceFromEntity(Device d) =>
        new(d.Id, d.ExactLocation, d.TypeOfMedication.ToString(), d.DoorStatus.ToString(),
            d.SensorReading.Temperature, d.SensorReading.Humidity,
            d.SensorReading.LightIntensity, d.SensorReading.AirQuality,
            d.SensorReading.Vibration, d.SensorReading.AtmosphericPressure,
            d.SensorReading.SuspendedParticles,
            d.EstablishmentId.Value, d.CreatedAt, d.EnabledSensors);
}
