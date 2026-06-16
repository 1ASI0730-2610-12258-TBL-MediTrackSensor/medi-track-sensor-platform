using Microsoft.AspNetCore.Mvc;
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Application.CommandServices;
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Domain.Model.Commands;
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Domain.Model.Errors;
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Interfaces.REST.Resources;
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Interfaces.REST.Transform;
using TechnoByteLambders.MediTrackSensor.Platform.Shared.Application.Patterns;
using ProblemDetailsFactory = TechnoByteLambders.MediTrackSensor.Platform.Shared.Interfaces.REST.ProblemDetails.ProblemDetailsFactory;

namespace TechnoByteLambders.MediTrackSensor.Platform.Iam.Interfaces.REST;

[ApiController]
[Route("api/v1/users")]
public class UsersController(
    IUserCommandService userCommandService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn(
        [FromBody] SignInResource resource,
        CancellationToken cancellationToken)
    {
        var result = await userCommandService.Handle(
            new SignInCommand(resource.Email, resource.Password),
            cancellationToken);

        return result switch
        {
            Result<(Domain.Model.Aggregates.User User, string Token), IamError>.Success success =>
                Ok(new AuthResource(
                    UserResourceFromEntityAssembler.ToResourceFromEntity(success.Value.User),
                    success.Value.Token)),
            Result<(Domain.Model.Aggregates.User User, string Token), IamError>.Failure failure =>
                failure.Error switch
                {
                    IamError.InvalidCredentials => problemDetailsFactory.CreateProblemDetails(
                        this,
                        StatusCodes.Status401Unauthorized,
                        IamError.InvalidCredentials,
                        IamErrors.InvalidCredentials.Description),
                    _ => problemDetailsFactory.CreateProblemDetails(
                        this,
                        StatusCodes.Status500InternalServerError,
                        IamError.InternalServerError,
                        IamErrors.InternalServerError.Description)
                },
            _ => problemDetailsFactory.CreateProblemDetails(
                this,
                StatusCodes.Status500InternalServerError,
                IamError.InternalServerError,
                IamErrors.InternalServerError.Description)
        };
    }
}
