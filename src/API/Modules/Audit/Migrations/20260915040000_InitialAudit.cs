using LogisticsPlatform.API.Audit;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LogisticsPlatform.API.Modules.Audit.Migrations;

[DbContext(typeof(AuditDbContext))]
[Migration("20260915040000_InitialAudit")]
public class InitialAudit : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema("audit");
        migrationBuilder.CreateTable("AuditLogs", "audit", table => new
        {
            Id = table.Column<Guid>(nullable: false), TenantId = table.Column<Guid>(nullable: true), UserId = table.Column<string>(nullable: true),
            Method = table.Column<string>(maxLength: 10, nullable: false), Path = table.Column<string>(maxLength: 500, nullable: false),
            StatusCode = table.Column<int>(nullable: false), CreatedAt = table.Column<DateTime>(nullable: false)
        }, table => table.PrimaryKey("PK_AuditLogs", item => item.Id));
        migrationBuilder.CreateIndex("IX_AuditLogs_TenantId_CreatedAt", "audit", "AuditLogs", new[] { "TenantId", "CreatedAt" });
    }
    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable("AuditLogs", "audit");
}
