using TechnoByteLambders.MediTrackSensor.Platform.Establishments.Application.CommandServices;
using TechnoByteLambders.MediTrackSensor.Platform.Establishments.Domain.Model.Aggregates;
using TechnoByteLambders.MediTrackSensor.Platform.Establishments.Domain.Model.Commands;
using TechnoByteLambders.MediTrackSensor.Platform.Establishments.Domain.Model.Errors;
using TechnoByteLambders.MediTrackSensor.Platform.Establishments.Domain.Repositories;
using TechnoByteLambders.MediTrackSensor.Platform.Shared.Application.Patterns;
using TechnoByteLambders.MediTrackSensor.Platform.Shared.Domain.Repositories;

namespace TechnoByteLambders.MediTrackSensor.Platform.Establishments.Application.Internal.CommandServices;

public class OperatorCommandService(
    IOperatorRepository operatorRepository,
    IUnitOfWork unitOfWork) : IOperatorCommandService
{
    public async Task<Result<Operator, EstablishmentsError>> Handle(
        UpdateOperatorCommand command,
        CancellationToken cancellationToken = default)
    {
        var op = await operatorRepository.FindByIdAsync(command.Id, cancellationToken);
        if (op is null)
            return new Result<Operator, EstablishmentsError>.Failure(EstablishmentsError.OperatorNotFound);

        op.UpdateSchedule(command.Schedule);

        try
        {
            operatorRepository.Update(op);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<Operator, EstablishmentsError>.Success(op);
        }
        catch (Exception)
        {
            return new Result<Operator, EstablishmentsError>.Failure(EstablishmentsError.OperatorUpdateFailed);
        }
    }
}
