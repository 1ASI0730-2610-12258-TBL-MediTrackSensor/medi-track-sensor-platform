using TechnoByteLambders.MediTrackSensor.Platform.Shared.Application.Patterns;
using TechnoByteLambders.MediTrackSensor.Platform.Subscriptions.Domain.Model.Errors;

namespace TechnoByteLambders.MediTrackSensor.Platform.Subscriptions.Application.CommandServices;

public interface ISubscriptionCommandService
{
    Task<Result<bool, SubscriptionsError>> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default);
}
