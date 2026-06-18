using TechnoByteLambders.MediTrackSensor.Platform.Iam.Domain.Model.Aggregates;
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Domain.Model.Commands;
using TechnoByteLambders.MediTrackSensor.Platform.Shared.Application.Patterns;

namespace TechnoByteLambders.MediTrackSensor.Platform.Iam.Application.CommandServices;

public interface IUserCommandService
{
    Task<Result<User, string>> Handle(SignUpCommand command, CancellationToken cancellationToken = default);
}