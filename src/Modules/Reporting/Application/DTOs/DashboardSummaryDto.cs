namespace LogisticsPlatform.Modules.Reporting.Application.DTOs;

public class DashboardSummaryDto
{
    public int TotalOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public int ActiveShipments { get; set; }
}
