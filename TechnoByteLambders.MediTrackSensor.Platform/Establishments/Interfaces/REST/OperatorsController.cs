using Microsoft.AspNetCore.Mvc;
using TechnoByteLambders.MediTrackSensor.Platform.Establishments.Application.CommandServices;
using TechnoByteLambders.MediTrackSensor.Platform.Establishments.Application.QueryServices;
using TechnoByteLambders.MediTrackSensor.Platform.Establishments.Domain.Model.Commands;
using TechnoByteLambders.MediTrackSensor.Platform.Establishments.Domain.Model.Errors;
using TechnoByteLambders.MediTrackSensor.Platform.Establishments.Domain.Model.Queries;
using TechnoByteLambders.MediTrackSensor.Platform.Establishments.Interfaces.REST.Resources;
using TechnoByteLambders.MediTrackSensor.Platform.Establishments.Interfaces.REST.Transform;
using TechnoByteLambders.MediTrackSensor.Platform.Shared.Application.Patterns;
using ProblemDetailsFactory = TechnoByteLambders.MediTrackSensor.Platform.Shared.Interfaces.REST.ProblemDetails.ProblemDetailsFactory;

namespace TechnoByteLambders.MediTrackSensor.Platform.Establishments.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
public class OperatorsController(
    IOperatorCommandService operatorCommandService,
    IOperatorQueryService queryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var items = await queryService.Handle(new GetAllOperatorsQuery(), ct);
        return Ok(items.Select(OperatorResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateOperatorResource resource,
        CancellationToken cancellationToken)
    {
        var result = await operatorCommandService.Handle(
            new UpdateOperatorCommand(id, resource.Schedule, resource.EstablishmentId),
            cancellationToken);

        return result switch
        {
            Result<Domain.Model.Aggregates.Operator, EstablishmentsError>.Success success =>
                Ok(OperatorResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),
            Result<Domain.Model.Aggregates.Operator, EstablishmentsError>.Failure failure =>
                failure.Error switch
                {
                    EstablishmentsError.OperatorNotFound => problemDetailsFactory.CreateProblemDetails(
                        this,
                        StatusCodes.Status404NotFound,
                        EstablishmentsError.OperatorNotFound,
                        EstablishmentsErrors.OperatorNotFound.Description),
                    EstablishmentsError.OperatorUpdateFailed => problemDetailsFactory.CreateProblemDetails(
                        this,
                        StatusCodes.Status400BadRequest,
                        EstablishmentsError.OperatorUpdateFailed,
                        EstablishmentsErrors.OperatorUpdateFailed.Description),
                    _ => problemDetailsFactory.CreateProblemDetails(
                        this,
                        StatusCodes.Status500InternalServerError,
                        EstablishmentsError.InternalServerError,
                        EstablishmentsErrors.InternalServerError.Description)
                },
            _ => problemDetailsFactory.CreateProblemDetails(
                this,
                StatusCodes.Status500InternalServerError,
                EstablishmentsError.InternalServerError,
                EstablishmentsErrors.InternalServerError.Description)
        };
    }
}
