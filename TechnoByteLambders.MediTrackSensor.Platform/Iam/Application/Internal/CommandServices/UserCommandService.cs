using TechnoByteLambders.MediTrackSensor.Platform.Iam.Application.CommandServices;
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Application.Internal.OutboundServices;
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Domain.Model.Aggregates;
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Domain.Model.Commands;
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Domain.Model.Errors;
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Domain.Repositories;
using TechnoByteLambders.MediTrackSensor.Platform.Shared.Application.Patterns;

namespace TechnoByteLambders.MediTrackSensor.Platform.Iam.Application.Internal.CommandServices;

public class UserCommandService(
    IUserRepository userRepository,
    IHashingService hashingService,
    ITokenService tokenService) : IUserCommandService
{
    public async Task<Result<(User User, string Token), IamError>> Handle(
        SignInCommand command,
        CancellationToken cancellationToken = default)
    {
        var email = command.Email.ToLowerInvariant().Trim();
        var user = await userRepository.FindByEmailAsync(email, cancellationToken);

        if (user is null || !hashingService.VerifyPassword(command.Password, user.PasswordHash))
            return new Result<(User, string), IamError>.Failure(IamError.InvalidCredentials);

        try
        {
            var token = tokenService.GenerateToken(user);
            return new Result<(User, string), IamError>.Success((user, token));
        }
        catch (Exception)
        {
            return new Result<(User, string), IamError>.Failure(IamError.InternalServerError);
        }
    }
}
