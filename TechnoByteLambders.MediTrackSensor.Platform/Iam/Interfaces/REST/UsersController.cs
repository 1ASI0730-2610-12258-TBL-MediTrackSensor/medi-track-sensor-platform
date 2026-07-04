using Microsoft.AspNetCore.Mvc;
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Application.QueryServices;
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Domain.Model.Queries;
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Interfaces.REST.Transform;

namespace TechnoByteLambders.MediTrackSensor.Platform.Iam.Interfaces.REST;

[ApiController]
[Route("api/v1/users")]
public class UsersController(IUserQueryService userQueryService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var users = await userQueryService.Handle(new GetAllUsersQuery(), ct);
        return Ok(users.Select(UserResourceFromEntityAssembler.ToResourceFromEntity));
    }
}
