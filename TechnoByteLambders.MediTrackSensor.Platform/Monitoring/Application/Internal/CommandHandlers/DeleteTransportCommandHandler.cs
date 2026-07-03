using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Application.Internal.Commands;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Domain.Repositories;
using TechnoByteLambders.MediTrackSensor.Platform.Shared.Domain.Repositories;

namespace TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Application.Internal.CommandHandlers;

public class DeleteTransportCommandHandler
{
    private readonly ITransportRepository _transportRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTransportCommandHandler(ITransportRepository transportRepository, IUnitOfWork unitOfWork)
    {
        _transportRepository = transportRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(DeleteTransportCommand command)
    {
        var transport = await _transportRepository.FindByIdAsync(command.Id);
        if (transport == null)
            throw new KeyNotFoundException($"Transporte con ID {command.Id} no encontrado.");

        _transportRepository.Remove(transport);
        await _unitOfWork.CompleteAsync();
    }
}
