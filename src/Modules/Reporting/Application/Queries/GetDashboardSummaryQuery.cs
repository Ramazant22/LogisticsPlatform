using System.Threading;
using System.Threading.Tasks;
using MediatR;
using LogisticsPlatform.Modules.Reporting.Application.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace LogisticsPlatform.Modules.Reporting.Application.Queries;

public class GetDashboardSummaryQuery : IRequest<DashboardSummaryDto>
{
}

public class GetDashboardSummaryQueryHandler : IRequestHandler<GetDashboardSummaryQuery, DashboardSummaryDto>
{
    private readonly string _connectionString;

    // Entity Framework yerine doðrudan SQL baðlantýsý alýyoruz (CQRS Query mantýðý)
    public GetDashboardSummaryQueryHandler(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
                            ?? "Server=localhost;Database=LogisticsDb;Trusted_Connection=True;TrustServerCertificate=True;";
    }

    public async Task<DashboardSummaryDto> Handle(GetDashboardSummaryQuery request, CancellationToken cancellationToken)
    {
        var result = new DashboardSummaryDto();

        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        // 1. Toplam Sipariþ Sayýsýný Al
        using (var command1 = new SqlCommand("SELECT COUNT(*) FROM orders.Orders", connection))
        {
            var count = await command1.ExecuteScalarAsync(cancellationToken);
            result.TotalOrders = count != null && count != DBNull.Value ? Convert.ToInt32(count) : 0;
        }

        // 2. Toplam Ciro (Kesilen Faturalar)
        using (var command2 = new SqlCommand("SELECT SUM(Amount) FROM billing.Invoices", connection))
        {
            var sum = await command2.ExecuteScalarAsync(cancellationToken);
            result.TotalRevenue = sum != null && sum != DBNull.Value ? Convert.ToDecimal(sum) : 0m;
        }

        // 3. Aktif Sevkiyatlar (Örnek olarak tüm sevkiyatlarý sayýyoruz)
        using (var command3 = new SqlCommand("SELECT COUNT(*) FROM shipment.Shipments", connection))
        {
            // Eðer Shipments tablosu veritabanýnda yoksa hata vermemesi için try-catch
            try 
            {
                var count = await command3.ExecuteScalarAsync(cancellationToken);
                result.ActiveShipments = count != null && count != DBNull.Value ? Convert.ToInt32(count) : 0;
            }
            catch { result.ActiveShipments = 0; }
        }

        return result;
    }
}
