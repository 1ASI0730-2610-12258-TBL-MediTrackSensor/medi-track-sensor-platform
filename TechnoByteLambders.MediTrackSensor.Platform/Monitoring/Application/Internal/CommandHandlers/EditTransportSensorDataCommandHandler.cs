using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Application.Internal.Commands;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Domain.Model.Aggregates;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Domain.Repositories;
using TechnoByteLambders.MediTrackSensor.Platform.Shared.Domain.Repositories; // Revisa si tu IUnitOfWork está aquí

namespace TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Application.Internal.CommandHandlers;

public class EditTransportSensorDataCommandHandler
{
    private readonly ITransportRepository _transportRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EditTransportSensorDataCommandHandler(ITransportRepository transportRepository, IUnitOfWork unitOfWork)
    {
        _transportRepository = transportRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(EditTransportSensorDataCommand command)
    {
        var transport = await _transportRepository.FindByIdAsync(command.TransportId);
        if (transport == null)
            throw new KeyNotFoundException($"Transporte con ID {command.TransportId} no encontrado.");

        transport.UpdateSensorData(command.Temperature, command.Humidity);

        _transportRepository.Update(transport);
        await _unitOfWork.CompleteAsync();
    }
}