using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Application.Internal.Queries;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Interfaces.REST.Resources;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Domain.Repositories;

namespace TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Application.Internal.CommandHandlers;

public class GetAllTransportsQueryHandler
{
    private readonly ITransportRepository _transportRepository;

    public GetAllTransportsQueryHandler(ITransportRepository transportRepository)
    {
        _transportRepository = transportRepository;
    }

    public async Task<IEnumerable<TransportResource>> HandleAsync(GetAllTransportsQuery query)
    {
        var transports = await _transportRepository.ListAsync();

        return transports.Select(t => new TransportResource(
            t.Id, 
            t.CurrentTemperature, 
            t.CurrentHumidity, 
            t.LastSensorUpdate
        ));
    }
}