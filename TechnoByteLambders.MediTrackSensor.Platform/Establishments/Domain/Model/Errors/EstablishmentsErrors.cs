using TechnoByteLambders.MediTrackSensor.Platform.Shared.Domain.Model;

namespace TechnoByteLambders.MediTrackSensor.Platform.Establishments.Domain.Model.Errors;

public static class EstablishmentsErrors
{
    public static readonly Error EstablishmentNotFound = new("Establishments.EstablishmentNotFound", "Establishment not found.");
}
