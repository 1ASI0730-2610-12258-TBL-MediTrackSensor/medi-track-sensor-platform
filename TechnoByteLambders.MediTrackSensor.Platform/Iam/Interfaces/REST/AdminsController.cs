using Microsoft.AspNetCore.Mvc;
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Application.QueryServices;
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Domain.Model.Queries;
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Interfaces.REST.Transform;

namespace TechnoByteLambders.MediTrackSensor.Platform.Iam.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
public class AdminsController(IAdminQueryService adminQueryService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var admins = await adminQueryService.Handle(new GetAllAdminsQuery(), ct);
        return Ok(admins.Select(AdminResourceFromEntityAssembler.ToResourceFromEntity));
    }
}
