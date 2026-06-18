namespace TechnoByteLambders.MediTrackSensor.Platform.Iam.Domain.Model.Commands;

public record CreateUserCommand(
    string Username, 
    string password,
    string Phone,
    string Email
    );
