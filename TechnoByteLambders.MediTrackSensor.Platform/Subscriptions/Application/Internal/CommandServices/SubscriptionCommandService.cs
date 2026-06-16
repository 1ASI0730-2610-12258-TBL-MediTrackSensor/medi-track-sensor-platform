using TechnoByteLambders.MediTrackSensor.Platform.Shared.Application.Patterns;
using TechnoByteLambders.MediTrackSensor.Platform.Shared.Domain.Repositories;
using TechnoByteLambders.MediTrackSensor.Platform.Subscriptions.Application.CommandServices;
using TechnoByteLambders.MediTrackSensor.Platform.Subscriptions.Domain.Model.Errors;
using TechnoByteLambders.MediTrackSensor.Platform.Subscriptions.Domain.Repositories;

namespace TechnoByteLambders.MediTrackSensor.Platform.Subscriptions.Application.Internal.CommandServices;

public class SubscriptionCommandService(
    ISubscriptionRepository subscriptionRepository,
    IUnitOfWork unitOfWork) : ISubscriptionCommandService
{
    public async Task<Result<bool, SubscriptionsError>> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(id, cancellationToken);
        if (subscription is null)
            return new Result<bool, SubscriptionsError>.Failure(SubscriptionsError.SubscriptionNotFound);

        try
        {
            subscriptionRepository.Remove(subscription);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<bool, SubscriptionsError>.Success(true);
        }
        catch (Exception)
        {
            return new Result<bool, SubscriptionsError>.Failure(SubscriptionsError.InternalServerError);
        }
    }
}
