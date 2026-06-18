using Microsoft.EntityFrameworkCore;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Domain.Model.Aggregates;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Domain.Model.ValueObjects;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Domain.Repositories;
using TechnoByteLambders.MediTrackSensor.Platform.Shared.Infrastructure.Persistence.EFC.Configuration;
using TechnoByteLambders.MediTrackSensor.Platform.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Infrastructure.Persistence.EFC.Repositories;

public class DeviceRepository(AppDbContext context)
    : BaseRepository<Device>(context), IDeviceRepository
{
    public async Task<IEnumerable<Device>> FindByEstablishmentIdAsync(
        int establishmentId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Device>()
            .Where(d => d.EstablishmentId == new EstablishmentId(establishmentId))
            .ToListAsync(cancellationToken);
    }
}
