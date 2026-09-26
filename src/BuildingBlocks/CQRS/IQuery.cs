using MediatR;

namespace LogisticsPlatform.BuildingBlocks.CQRS;

public interface IQuery<out TResponse> : IRequest<TResponse> { }
