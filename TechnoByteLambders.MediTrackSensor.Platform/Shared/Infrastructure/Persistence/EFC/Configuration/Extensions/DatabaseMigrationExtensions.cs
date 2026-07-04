using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TechnoByteLambders.MediTrackSensor.Platform.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class DatabaseMigrationExtensions
{
    private static readonly (string MigrationId, string TableName, string ProductVersion)[] KnownMigrations =
    [
        ("20260609120000_AddUsersTable", "users", "9.0.6"),
        ("20260610130000_AddDevicesTable", "devices", "9.0.6"),
        ("20260702093209_AddEstablishmentsOperatorsSubscriptions", "establishments", "9.0.6"),
        ("20260703042413_AddTransportsTable", "transports", "10.0.1"),
        ("20260704130000_AddAdminsTable", "admins", "10.0.1"),
    ];

    public static void ApplyPendingMigrations(this DatabaseFacade database)
    {
        SyncMigrationHistoryIfNeeded(database);
        database.GetService<IMigrator>().Migrate();
    }

    private static void SyncMigrationHistoryIfNeeded(DatabaseFacade database)
    {
        database.ExecuteSqlRaw("""
            CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
                `MigrationId` varchar(150) NOT NULL,
                `ProductVersion` varchar(32) NOT NULL,
                PRIMARY KEY (`MigrationId`)
            );
            """);

        foreach (var (migrationId, tableName, productVersion) in KnownMigrations)
        {
            if (!TableExists(database, tableName) || MigrationExists(database, migrationId))
                continue;

            database.ExecuteSqlRaw(
                "INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`) VALUES ({0}, {1})",
                migrationId,
                productVersion);
        }
    }

    private static bool TableExists(DatabaseFacade database, string tableName)
    {
        var connection = database.GetDbConnection();
        var shouldClose = connection.State != System.Data.ConnectionState.Open;
        if (shouldClose)
            connection.Open();

        try
        {
            using var command = connection.CreateCommand();
            command.CommandText =
                "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = DATABASE() AND table_name = @tableName";
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@tableName";
            parameter.Value = tableName;
            command.Parameters.Add(parameter);
            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }
        finally
        {
            if (shouldClose)
                connection.Close();
        }
    }

    private static bool MigrationExists(DatabaseFacade database, string migrationId)
    {
        var connection = database.GetDbConnection();
        var shouldClose = connection.State != System.Data.ConnectionState.Open;
        if (shouldClose)
            connection.Open();

        try
        {
            using var command = connection.CreateCommand();
            command.CommandText =
                "SELECT COUNT(*) FROM `__EFMigrationsHistory` WHERE `MigrationId` = @migrationId";
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@migrationId";
            parameter.Value = migrationId;
            command.Parameters.Add(parameter);
            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }
        finally
        {
            if (shouldClose)
                connection.Close();
        }
    }
}
