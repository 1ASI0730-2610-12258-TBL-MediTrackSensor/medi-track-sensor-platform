using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Domain.Model.Aggregates;

namespace TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Domain.Repositories;

public interface ITransportRepository
{
    Task AddAsync(Transport transport);
    Task<Transport?> FindByIdAsync(Guid id);
    void Update(Transport transport);
    Task<IEnumerable<Transport>> ListAsync();
}