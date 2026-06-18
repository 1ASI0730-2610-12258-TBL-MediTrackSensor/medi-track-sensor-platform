using TechnoByteLambders.MediTrackSensor.Platform.Iam.Domain.Model.Aggregates;
using TechnoByteLambders.MediTrackSensor.Platform.Shared.Application.Patterns;

namespace TechnoByteLambders.MediTrackSensor.Platform.Iam.Application.CommandServices;

public interface IUserCommandService
{
    Task<Result<bool, string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
}