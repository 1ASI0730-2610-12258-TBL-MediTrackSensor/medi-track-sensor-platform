using TechnoByteLambders.MediTrackSensor.Platform.Shared.Domain.Model;

namespace TechnoByteLambders.MediTrackSensor.Platform.Iam.Domain.Model.Errors;

public static class IamErrors
{
    public static readonly Error InvalidCredentials = new(
        "Iam.InvalidCredentials",
        "Invalid email or password.");

    public static readonly Error InternalServerError = new(
        "Iam.InternalServerError",
        "An unexpected error occurred.");
}
