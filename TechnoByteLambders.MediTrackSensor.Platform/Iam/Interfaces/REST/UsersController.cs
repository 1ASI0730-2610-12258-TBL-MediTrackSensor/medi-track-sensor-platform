    using Microsoft.AspNetCore.Mvc;
    using TechnoByteLambders.MediTrackSensor.Platform.Iam.Application.CommandServices;
    using TechnoByteLambders.MediTrackSensor.Platform.Iam.Domain.Model.Aggregates;
    using TechnoByteLambders.MediTrackSensor.Platform.Iam.Domain.Model.Commands;
    using TechnoByteLambders.MediTrackSensor.Platform.Iam.Interfaces.REST.Resources;
    using TechnoByteLambders.MediTrackSensor.Platform.Iam.Interfaces.REST.Transform;
    using TechnoByteLambders.MediTrackSensor.Platform.Shared.Application.Patterns;

    namespace TechnoByteLambders.MediTrackSensor.Platform.Iam.Interfaces.REST;

    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController(IUserCommandService userCommandService) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] SignUpResource resource, CancellationToken ct)
        {
            var command = SignUpCommandFromResourceAssembler.ToCommandFromResource(resource);
            
            var result = await userCommandService.Handle(command, ct);

            if (result is Result<User, string>.Failure f)
                return BadRequest(new { error = f.Error });
            var success = (Result<User, string>.Success)result;
            
            return Created(
                $"/api/v1/users/{success.Value.Id}",
                UserResourceFromEntityAssembler.ToResourceFromEntity(success.Value)
            );
        }

    }