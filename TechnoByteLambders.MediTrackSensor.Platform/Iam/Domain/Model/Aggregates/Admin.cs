using TechnoByteLambders.MediTrackSensor.Platform.Iam.Domain.Model.ValueObjects;

namespace TechnoByteLambders.MediTrackSensor.Platform.Iam.Domain.Model.Aggregates;

public partial class Admin(string entityName, string entityCode, string schedule, UserId userId)
{
    public Admin() : this(string.Empty, string.Empty, string.Empty, new UserId(0))
    {
    }

    public int Id { get; }
    public string EntityName { get; private set; } = entityName;
    public string EntityCode { get; private set; } = entityCode;
    public string Schedule { get; private set; } = schedule;
    public UserId UserId { get; private set; } = userId;
}
