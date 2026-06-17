using TechnoByteLambders.MediTrackSensor.Platform.Establishments.Application.CommandServices;
using TechnoByteLambders.MediTrackSensor.Platform.Establishments.Domain.Model.Errors;
using TechnoByteLambders.MediTrackSensor.Platform.Establishments.Domain.Repositories;
using TechnoByteLambders.MediTrackSensor.Platform.Shared.Application.Patterns;
using TechnoByteLambders.MediTrackSensor.Platform.Shared.Domain.Repositories;

namespace TechnoByteLambders.MediTrackSensor.Platform.Establishments.Application.Internal.CommandServices;

public class EstablishmentCommandService(
    IEstablishmentRepository establishmentRepository,
    IUnitOfWork unitOfWork) : IEstablishmentCommandService
{
    public async Task<Result<bool, string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var establishment = await establishmentRepository.FindByIdAsync(id, cancellationToken);
        if (establishment is null)
            return new Result<bool, string>.Failure(EstablishmentsErrors.EstablishmentNotFound.Description);

        establishmentRepository.Remove(establishment);
        await unitOfWork.CompleteAsync(cancellationToken);
        return new Result<bool, string>.Success(true);
    }
}
