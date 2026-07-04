using Microsoft.AspNetCore.Mvc;
using TechnoByteLambders.MediTrackSensor.Platform.Subscriptions.Application.QueryServices;
using TechnoByteLambders.MediTrackSensor.Platform.Subscriptions.Domain.Model.Queries;
using TechnoByteLambders.MediTrackSensor.Platform.Subscriptions.Interfaces.REST.Transform;

namespace TechnoByteLambders.MediTrackSensor.Platform.Subscriptions.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
public class SubscriptionsController(ISubscriptionQueryService queryService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var items = await queryService.Handle(new GetAllSubscriptionsQuery(), ct);
        return Ok(items.Select(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity));
    }
}
