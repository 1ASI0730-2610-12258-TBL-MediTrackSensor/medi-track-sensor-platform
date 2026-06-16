using TechnoByteLambders.MediTrackSensor.Platform.Shared.Domain.Model;

namespace TechnoByteLambders.MediTrackSensor.Platform.Establishments.Domain.Model.Errors;

public static class EstablishmentsErrors
{
    public static readonly Error EstablishmentCreationFailed = new(
        "Establishments.EstablishmentCreationFailed",
        "An error occurred while creating the establishment.");

    public static readonly Error OperatorNotFound = new(
        "Establishments.OperatorNotFound",
        "Operator not found.");

    public static readonly Error OperatorUpdateFailed = new(
        "Establishments.OperatorUpdateFailed",
        "An error occurred while updating the operator.");

    public static readonly Error InternalServerError = new(
        "Establishments.InternalServerError",
        "An unexpected error occurred.");
}
