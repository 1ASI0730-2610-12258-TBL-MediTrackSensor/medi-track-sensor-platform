using System;

namespace TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Domain.Model.Aggregates;

public class Transport
{
    public Guid Id { get; private set; }
    public decimal CurrentTemperature { get; private set; }
    public decimal CurrentHumidity { get; private set; }
    public DateTime LastSensorUpdate { get; private set; }

    private Transport() {}

    public Transport(Guid id)
    {
        Id = id;
    }

    public void UpdateSensorData(decimal temperature, decimal humidity)
    {
        CurrentTemperature = temperature;
        CurrentHumidity = humidity;
        LastSensorUpdate = DateTime.UtcNow;
    }
}