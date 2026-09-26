using MediatR;
namespace LogisticsPlatform.BuildingBlocks.Domain;

public interface IDomainEvent : INotification
{
    Guid EventId { get; }
    DateTime OccurredOn { get; }
}
