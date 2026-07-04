using Microsoft.EntityFrameworkCore;
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Domain.Model.Aggregates;
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Domain.Model.ValueObjects;

namespace TechnoByteLambders.MediTrackSensor.Platform.Iam.Infrastructure.Persistence.EFC.Configuration;

public static class IamContextConfiguration
{
    public static void ApplyIamConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Admin>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id).HasColumnName("id");
            entity.Property(a => a.EntityName).HasColumnName("entity_name").IsRequired().HasMaxLength(400);
            entity.Property(a => a.EntityCode).HasColumnName("entity_code").IsRequired().HasMaxLength(20);
            entity.Property(a => a.Schedule).HasColumnName("schedule").HasMaxLength(100);

            entity.Property(a => a.UserId)
                .HasConversion(v => v.Value, v => new UserId(v))
                .HasColumnName("users_id");
        });
    }
}
