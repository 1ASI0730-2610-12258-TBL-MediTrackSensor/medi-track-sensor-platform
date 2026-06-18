using Microsoft.EntityFrameworkCore;
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Application.CommandServices;
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Application.Internal.OutboundServices;
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Domain.Model.Aggregates;
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Domain.Model.Commands;
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Domain.Model.Errors;
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Domain.Repositories;
using TechnoByteLambders.MediTrackSensor.Platform.Shared.Application.Patterns;
using TechnoByteLambders.MediTrackSensor.Platform.Shared.Domain.Repositories;



namespace TechnoByteLambders.MediTrackSensor.Platform.Iam.Application.Internal.CommandServices;

public class UserCommandService(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IHashingService hashingService,
    ITokenService tokenService) : IUserCommandService
{
    public async Task<Result<User, string>> Handle(SignUpCommand command, CancellationToken cancellationToken = default)
    {
        var email = command.Email.ToLowerInvariant().Trim();
        if (await userRepository.ExistsByEmailAsync(email, cancellationToken))
            return new Result<User, string>.Failure(IamErrors.UserAlreadyExists.Description);

        var passwordHash = hashingService.HashPassword(command.Password);
        var user = new User(command with { Email = email }, passwordHash);
        try
        {
            await userRepository.AddAsync(user, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<User, string>.Success(user);
        }
        catch (OperationCanceledException) { return new Result<User, string>.Failure(IamErrors.UserCreationFailed.Description); }
        catch (DbUpdateException) { return new Result<User, string>.Failure(IamErrors.UserCreationFailed.Description); }
        catch (Exception) { return new Result<User, string>.Failure(IamErrors.UserCreationFailed.Description); }
    }

 

  
}