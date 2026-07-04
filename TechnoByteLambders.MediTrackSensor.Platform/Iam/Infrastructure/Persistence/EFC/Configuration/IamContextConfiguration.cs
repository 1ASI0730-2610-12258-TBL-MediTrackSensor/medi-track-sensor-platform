using Microsoft.EntityFrameworkCore;
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Domain.Model.Aggregates;
using TechnoByteLambders.MediTrackSensor.Platform.Iam.Domain.Model.ValueObjects;

namespace TechnoByteLambders.MediTrackSensor.Platform.Iam.Infrastructure.Persistence.EFC.Configuration;

public static class IamContextConfiguration
{
    public static void ApplyIamConfiguration(this ModelBuilder builder)
    {
        builder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).HasColumnName("id");
            entity.Property(u => u.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
            entity.Property(u => u.Phone).HasColumnName("phone").HasMaxLength(15);
            entity.Property(u => u.JobTitle).HasColumnName("job_title").HasMaxLength(100);
            entity.Property(u => u.EntryDate)
                .HasColumnName("entry_date")
                .HasConversion(v => v.ToDateTime(TimeOnly.MinValue), v => DateOnly.FromDateTime(v))
                .IsRequired();
            entity.Property(u => u.Role).HasColumnName("role").IsRequired().HasConversion<string>();
            entity.Property(u => u.PasswordHash).HasColumnName("password_hash").IsRequired().HasMaxLength(255);
            entity.Property(u => u.Photo).HasColumnName("photo").HasMaxLength(255);

            entity.Property(u => u.Dni)
                .HasConversion(v => v.Value, v => new Dni(v))
                .HasColumnName("dni")
                .HasMaxLength(8);

            entity.Property(u => u.Email)
                .HasConversion(v => v.Value, v => new Email(v))
                .HasColumnName("email")
                .HasMaxLength(100);
        });

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
