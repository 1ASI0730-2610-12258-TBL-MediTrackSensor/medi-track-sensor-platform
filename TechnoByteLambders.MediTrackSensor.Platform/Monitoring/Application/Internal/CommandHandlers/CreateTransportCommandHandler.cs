using System;
using System.Threading.Tasks;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Application.Internal.Commands;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Domain.Model.Aggregates;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Domain.Repositories;
using TechnoByteLambders.MediTrackSensor.Platform.Shared.Domain.Repositories;

namespace TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Application.Internal.CommandHandlers;

public class CreateTransportCommandHandler
{
    private readonly ITransportRepository _transportRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTransportCommandHandler(ITransportRepository transportRepository, IUnitOfWork unitOfWork)
    {
        _transportRepository = transportRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Transport> HandleAsync(CreateTransportCommand command)
    {
        var transport = new Transport(Guid.NewGuid());
        transport.UpdateSensorData(command.Temperature, command.Humidity);

        await _transportRepository.AddAsync(transport);
        await _unitOfWork.CompleteAsync();

        return transport;
    }
}
