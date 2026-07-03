using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Domain.Model.Aggregates;
using TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Domain.Repositories;
// Cambia esto por el namespace real donde esté tu DbContext (suele estar en Shared o la raíz de Infrastructure)
using TechnoByteLambders.MediTrackSensor.Platform.Shared.Infrastructure.Persistence.EFC.Configuration; 

namespace TechnoByteLambders.MediTrackSensor.Platform.Monitoring.Infrastructure.Persistence.EFC.Repositories;

public class TransportRepository : ITransportRepository
{
    // Reemplaza 'AppDbContext' por el nombre exacto de tu clase DbContext del proyecto
    private readonly AppDbContext _context; 

    public TransportRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Transport transport)
    {
        await _context.Set<Transport>().AddAsync(transport);
    }

    public async Task<Transport?> FindByIdAsync(Guid id)
    {
        return await _context.Set<Transport>().FindAsync(id);
    }

    public void Update(Transport transport)
    {
        _context.Set<Transport>().Update(transport);
    }

    public async Task<IEnumerable<Transport>> ListAsync()
    {
        return await _context.Set<Transport>().ToListAsync();
    }
}