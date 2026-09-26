DROP TABLE IF EXISTS [tenancy].[Tenants];
DROP SCHEMA IF EXISTS [tenancy];
DELETE FROM [__EFMigrationsHistory] WHERE [MigrationId] LIKE '%Tenanc%';
DELETE FROM [__EFMigrationsHistory] WHERE [MigrationId] LIKE '%Tenant%';