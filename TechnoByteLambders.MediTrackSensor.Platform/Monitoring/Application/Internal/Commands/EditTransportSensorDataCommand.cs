using System;

namespace TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Application.Internal.Commands;

public record EditTransportSensorDataCommand(Guid TransportId, decimal Temperature, decimal Humidity);