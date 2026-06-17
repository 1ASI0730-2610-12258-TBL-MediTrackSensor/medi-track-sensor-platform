using TechnoByteLambders.MediTrackSensor.Platform.Establishments.Domain.Model.Aggregates;
using TechnoByteLambders.MediTrackSensor.Platform.Establishments.Interfaces.REST.Resources;

namespace TechnoByteLambders.MediTrackSensor.Platform.Establishments.Interfaces.REST.Transform;

public static class OperatorResourceFromEntityAssembler
{
    public static OperatorResource ToResourceFromEntity(Operator o) =>
        new(o.Id, o.AlertsAnswered, o.Schedule, o.EstablishmentId, o.UserId.Value);
}
