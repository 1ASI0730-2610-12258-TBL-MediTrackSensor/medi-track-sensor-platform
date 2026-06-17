using TechnoByteLambders.MediTrackSensor.Platform.Shared.Application.Patterns;

namespace TechnoByteLambders.MediTrackSensor.Platform.Establishments.Application.CommandServices;

public interface IEstablishmentCommandService
{
    Task<Result<bool, string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
