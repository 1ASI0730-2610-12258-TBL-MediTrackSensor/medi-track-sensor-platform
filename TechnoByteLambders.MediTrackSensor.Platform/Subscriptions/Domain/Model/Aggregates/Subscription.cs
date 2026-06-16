using TechnoByteLambders.MediTrackSensor.Platform.Subscriptions.Domain.Model.ValueObjects;

namespace TechnoByteLambders.MediTrackSensor.Platform.Subscriptions.Domain.Model.Aggregates;

public partial class Subscription(
    SubscriptionPlan plan,
    DateOnly startDate,
    DateOnly endDate,
    SubscriptionStatus status,
    AdminId adminId)
{
    public Subscription() : this(
        SubscriptionPlan.Free,
        DateOnly.MinValue,
        DateOnly.MinValue,
        SubscriptionStatus.Pending,
        new AdminId(0))
    {
    }

    public int Id { get; }
    public SubscriptionPlan Plan { get; private set; } = plan;
    public DateOnly StartDate { get; private set; } = startDate;
    public DateOnly EndDate { get; private set; } = endDate;
    public SubscriptionStatus Status { get; private set; } = status;
    public AdminId AdminId { get; private set; } = adminId;
}
