namespace LogisticsPlatform.BuildingBlocks.Domain;

public interface ICurrentUserService
{
    Guid UserId { get; }
    Guid TenantId { get; }
    bool IsAuthenticated { get; }
}
