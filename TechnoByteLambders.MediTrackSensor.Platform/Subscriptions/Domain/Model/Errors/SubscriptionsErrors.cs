using TechnoByteLambders.MediTrackSensor.Platform.Shared.Domain.Model;

namespace TechnoByteLambders.MediTrackSensor.Platform.Subscriptions.Domain.Model.Errors;

public static class SubscriptionsErrors
{
    public static readonly Error SubscriptionNotFound = new(
        "Subscriptions.SubscriptionNotFound",
        "Subscription not found.");

    public static readonly Error InternalServerError = new(
        "Subscriptions.InternalServerError",
        "An unexpected error occurred.");
}
