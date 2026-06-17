using TechnoByteLambders.MediTrackSensor.Platform.Establishments.Domain.Model.ValueObjects;

namespace TechnoByteLambders.MediTrackSensor.Platform.Establishments.Domain.Model.Aggregates;

public partial class Operator(int alertsAnswered, string schedule, int establishmentId, UserId userId)
{
    public int Id { get; }
    public int AlertsAnswered { get; private set; } = alertsAnswered;
    public string Schedule { get; private set; } = schedule;
    public int EstablishmentId { get; private set; } = establishmentId;
    public UserId UserId { get; private set; } = userId;
}
