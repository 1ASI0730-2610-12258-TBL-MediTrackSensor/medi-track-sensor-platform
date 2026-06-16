using Microsoft.AspNetCore.Mvc;
using TechnoByteLambders.MediTrackSensor.Platform.Shared.Application.Patterns;
using TechnoByteLambders.MediTrackSensor.Platform.Subscriptions.Application.CommandServices;
using TechnoByteLambders.MediTrackSensor.Platform.Subscriptions.Domain.Model.Errors;
using ProblemDetailsFactory = TechnoByteLambders.MediTrackSensor.Platform.Shared.Interfaces.REST.ProblemDetails.ProblemDetailsFactory;

namespace TechnoByteLambders.MediTrackSensor.Platform.Subscriptions.Interfaces.REST;

[ApiController]
[Route("api/v1/subscriptions")]
public class SubscriptionsController(
    ISubscriptionCommandService subscriptionCommandService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await subscriptionCommandService.DeleteAsync(id, cancellationToken);

        return result switch
        {
            Result<bool, SubscriptionsError>.Success => NoContent(),
            Result<bool, SubscriptionsError>.Failure failure =>
                failure.Error switch
                {
                    SubscriptionsError.SubscriptionNotFound => problemDetailsFactory.CreateProblemDetails(
                        this,
                        StatusCodes.Status404NotFound,
                        SubscriptionsError.SubscriptionNotFound,
                        SubscriptionsErrors.SubscriptionNotFound.Description),
                    _ => problemDetailsFactory.CreateProblemDetails(
                        this,
                        StatusCodes.Status500InternalServerError,
                        SubscriptionsError.InternalServerError,
                        SubscriptionsErrors.InternalServerError.Description)
                },
            _ => problemDetailsFactory.CreateProblemDetails(
                this,
                StatusCodes.Status500InternalServerError,
                SubscriptionsError.InternalServerError,
                SubscriptionsErrors.InternalServerError.Description)
        };
    }
}
